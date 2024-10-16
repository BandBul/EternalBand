﻿using EternalBAND.Common;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using EternalBAND.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using EternalBAND.Api.Exceptions;
using System.Collections;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using JsonException = EternalBAND.Api.Exceptions.JsonException;
using EternalBAND.Common;
using X.PagedList.EF;

namespace EternalBAND.Api.Services
{
    public class UserService
    {
        private readonly BroadCastingManager _notificationManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IWebHostEnvironment _environment;

        public UserService(ApplicationDbContext context, UserManager<Users> userManager, IWebHostEnvironment environment, BroadCastingManager notificationManager)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _notificationManager = notificationManager;
        }
        public async Task<IEnumerable<Posts>> PostIndex(Users currentUser)
        {
            return _context.Posts.Where(n => n.AddedByUserId == currentUser.Id)
                .Include(p => p.AddedByUser)
                .Include(p => p.AdminConfirmationUser)
                .Include(p => p.PostTypes)
                .Include(p => p.Instruments);
        }


        public async Task PostCreate(Users? currentUser, Posts post, List<IFormFile>? images)
        {
            await PostAddPhoto(post, images);
            PostAddSeoLink(post);
            post.AddedByUserId = currentUser?.Id;
            post.AddedDate = DateTime.Now;
            post.Guid = Guid.NewGuid();
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, Constants.AdminRoleName);
            post.Status = isAdmin ? PostStatus.Active : PostStatus.PendingApproval;
            if (!isAdmin)
            {
                var adminMessage = $"{currentUser?.Name} '{post.SeoLink}' id li yeni bir ilan paylaştı.";
                var userMessage = $"<strong>'{post.Title}'</strong> başlıklı ilanınız onay sürecindedir. En kısa sürede onay sürecini tamamlayacağız";
                await _notificationManager.CreateAdminNotification(currentUser, post, adminMessage);
                await _notificationManager.CreateUserNotification(currentUser, post, userMessage);
            }
          
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Posts> PostEditInitial(Guid? postId)
        {
            if (postId == null || _context.Posts == null)
            {
                throw new NotFoundException();
            }

            var posts = await _context.Posts.FirstOrDefaultAsync(n => n.Guid == postId);
            if (posts == null)
            {
                throw new NotFoundException();
            }

            return posts;
        }

        public async Task PostEdit(Users currentUser, Posts posts, List<IFormFile>? images)
        {
            try
            {
                if (posts.Guid == null)
                {
                    throw new NotFoundException();
                }

                PostAddPhoto(posts, images);
                var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(n => n.Guid == posts.Guid);
                if ((currentUser.Id == post.AddedByUserId))
                {
                    var isAdmin = await _userManager.IsInRoleAsync(currentUser, Constants.AdminRoleName);
                    if (images.Count == 0)
                    {
                        posts.Photo1 = post.Photo1;
                        posts.Photo2 = post.Photo2;
                        posts.Photo3 = post.Photo3;
                        posts.Photo4 = post.Photo4;
                        posts.Photo5 = post.Photo5;
                    }

                    posts.Id = post.Id;
                    posts.AddedByUserId = post.AddedByUserId;
                    posts.AdminConfirmationUserId = post.AdminConfirmationUserId;
                    posts.AdminConfirmation = post.AdminConfirmation;
                    posts.AddedDate = post.AddedDate;
                    posts.SeoLink = post.SeoLink;
                    posts.Guid = post.Guid;
                    posts.Status = isAdmin ? PostStatus.Active : PostStatus.PendingApproval;
                    if (!isAdmin)
                    {
                        posts.Status = PostStatus.PendingApproval;
                        var message = $"{currentUser?.Name} '{posts.SeoLink}' ilanında güncelleme yaptı";
                        await _notificationManager.CreateAdminNotification(currentUser, post, message);
                        var userMessage = $"<strong>'{post.Title}'</strong> başlıklı ilanınız onay sürecindedir. En kısa sürede onay sürecini tamamlayacağız";
                        await _notificationManager.CreateUserNotification(currentUser, post, userMessage);
                    }

                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsPostsExists(posts.Id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    // TODO log
                    throw;
                }
            }
        }

        public async Task PostArchived(Users currentUser, int postId)
        {
            var post = _context.Posts.Find(postId);
            if (post == null)
            {
                throw new BadRequestException($"There is no post with  this id : {postId}");
                //return BadRequest($"There is no post with  this id : {id}");
            }

            post.Status = PostStatus.DeActive;
            _context.Posts.Update(post);

            await _context.SaveChangesAsync();
        }

        public async Task PostActivated(Users currentUser, int postId)
        {
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, Constants.AdminRoleName);
            var post = _context.Posts.Find(postId);

            if (post == null)
            {
                throw new BadRequestException($"There is no post with  this id : {postId}");
            }

            post.Status = isAdmin ? PostStatus.Active : PostStatus.PendingApproval;
            _context.Posts.Update(post);
            if (!isAdmin)
            {
                var message = $"{currentUser?.Name} '{post.SeoLink}' ilanını arşivden yayına almak istiyor";
                await _notificationManager.CreateAdminNotification(currentUser, post, message);
                var userMessage = $"<strong>'{post.Title}'</strong> başlıklı ilanınız onay sürecindedir. En kısa sürede onay sürecini tamamlayacağız";
                await _notificationManager.CreateUserNotification(currentUser, post, userMessage);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> PostDelete(Users currentUser, int postId)
        {
            if (_context.Posts == null)
            {
                throw new JsonException("Kayıt Bulunamadı.");
            }

            var posts = _context.Posts.Include(s => s.MessageBoxes).FirstOrDefault(s => s.Id == postId);

            if (posts == null)
            {
                throw new JsonException("İlan bulunamadı.");
            }

            if ((currentUser.Id != posts.AddedByUserId) || currentUser.Id == null)
            {
                throw new JsonException("Bu ilan size ait değil.");
            }
            else
            {
                _context.Posts.Remove(posts);
                var messageBoxes = posts.MessageBoxes;
                foreach (var mb in messageBoxes)
                {
                    mb.IsPostDeleted = true;
                }
                await _context.SaveChangesAsync();


                return "İlan silindi.";
            }
        }

        public async Task<string> AllReadNotification(Users currentUser)
        {
            var notReadedNotifs = await _context.Notification
                .Where(n => n.ReceiveUserId == currentUser.Id && !n.IsRead).ToListAsync();

            foreach (var noti in notReadedNotifs)
            {
                noti.IsRead = true;
            }
            await _context.SaveChangesAsync();
            return "İşlem başarılı.";
        }


        public async Task<IPagedList<Notification>> NotificationIndex(Users currentUser, int pageId)
        {
            return await _context.Notification.Where(n => n.ReceiveUserId == currentUser.Id && n.NotificationType == NotificationType.PostSharing)
                .OrderByDescending(n => n.IsRead)
                .ThenByDescending(n => n.AddedDate)
                .ToPagedListAsync(pageId, 10);
        }

        public async Task<string> NotificationRead(int notifId, Users user)
        {
            var notif = _context.Notification.Find(notifId);

            if (notif == null)
            {
                throw new BadRequestException($"There is no notification with this id : {notifId}");
            }

            if(!notif.IsRead)
            {
                notif.IsRead = true;
                _context.Notification.Update(notif);
                await _context.SaveChangesAsync();
            }

            if (notif.NotificationType == NotificationType.PostSharing)
            {
                string warningMessage = "";
                if(TryToGetWarningMessage(notif.RelatedElementId, user, out warningMessage))
                {
                    throw new JsonException(warningMessage);
                }
            }

            return notif.RedirectLink;
        }

        public IEnumerable<PostTypes> GetPostTypes() 
        {
            var postTypes = _context.PostTypes.ToList();
            return postTypes.Select(s =>
                {
                    var newTitle = s.TypeText + $" ({s.PostCreateText})";
                    return new PostTypes()
                    {
                        Id = s.Id,
                        FilterText = newTitle
                    };
                }
            );
        }
        public IEnumerable<Instruments> GetInstruments()
        {
            return _context.Instruments;
        }

        private bool TryToGetWarningMessage(string seoLink, Users user, out string warningMessage)
        {
            var post = _context.Posts.FirstOrDefault(s => s.SeoLink.Equals(seoLink));
            
            if (post == null)
            {
                warningMessage = "İlgili ilan silindiği için yayında değildir";
                return true;
            }

            else
            {
                var isPostYourOwn = IsCurrentUserOwnerOfPost(post, user);
                var title = post.Title;
                if (isPostYourOwn || (post.Status == PostStatus.Active || post.Status == PostStatus.PendingApproval))
                {
                    warningMessage = "";
                    return false;
                }

                else if (post.Status == PostStatus.DeActive)
                {
                    warningMessage = $"'{title}' başlıklı ilan, kullanıcı tarafından arşive alınmıştır.";
                }
                else if (post.Status == PostStatus.Rejected)
                {
                    warningMessage = $"'{title}' başlıklı ilan, düzenleme gerektirdiği için arşivdedir";
                }
                else
                {
                    warningMessage = $"'{title}' başlıklı ilan yayından kaldırılmıştır";
                }
                return true;
            }
            
        }

        private bool IsCurrentUserOwnerOfPost(Posts post, Users user)
        {
            return post.AddedByUserId == user.Id;
        }

        private bool IsPostsExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task PostAddPhoto(Posts posts, List<IFormFile>? images)
        {
            if (images != null)
            {
                foreach (var image in images)
                {
                    var photoName = new PhotoName().GeneratePhotoName(new Random().Next(0, 10000000).ToString()) +
                                    new Random().Next(0, 1000) +
                                    System.IO.Path.GetExtension(image.FileName);
                    using (var stream =
                           new FileStream(
                               Path.Combine(_environment.WebRootPath, "images/ilan/", photoName),
                               FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                        if (posts.Photo1 == null)
                        {
                            posts.Photo1 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (posts.Photo2 == null)
                        {
                            posts.Photo2 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (posts.Photo3 == null)
                        {
                            posts.Photo3 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (posts.Photo4 == null)
                        {
                            posts.Photo4 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (posts.Photo5 == null)
                        {
                            posts.Photo5 = "/images/ilan/" + photoName;
                            continue;
                        }
                    }
                }
            }
        }

        private async Task PostAddSeoLink(Posts post)
        {
            var parsedStr = StrConvert.TRToEnDeleteAllSpacesAndToLower(post.Title);
            string seoLink = 
                parsedStr 
                + new Random().Next(0, 9999999) 
                + new Random().Next(0, 9999);
            while (true)
            {
                if (!_context.Posts.Any(n => n.SeoLink == seoLink))
                {
                    post.SeoLink = seoLink;
                    break;
                }
            }
        }
    }
}
