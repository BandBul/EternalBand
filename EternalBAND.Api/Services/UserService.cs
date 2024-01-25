using EternalBAND.Common;
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

namespace EternalBAND.Api.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IWebHostEnvironment _environment;

        public UserService(ApplicationDbContext context, UserManager<Users> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }
        public async Task<List<Posts>> PostIndex(Users currentUser)
        {
            return _context.Posts.Where(n => n.AddedByUserId == currentUser.Id)
                .Include(p => p.AddedByUser)
                .Include(p => p.AdminConfirmationUser)
                .Include(p => p.PostTypes)
                .Include(p => p.Instruments)
                .ToList();
        }


        public async Task PostCreate(Users? currentUser, Posts posts, List<IFormFile>? images)
        {
            await PostAddPhoto(posts, images);
            PostAddSeoLink(posts);
            posts.AddedByUserId = currentUser?.Id;
            posts.AddedDate = DateTime.Now;
            posts.Guid = Guid.NewGuid();
            var isUserAdmin = await _userManager.IsInRoleAsync(currentUser, Constants.AdminRoleName);
            if (!isUserAdmin)
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
                // Engin-TODO need to send all admins
                // Engin-TODO pass SignalR hub and use ReceiveNotification broadcasting title to send message to front end
                await _context.Notification.AddAsync(new Notification()
                {
                    IsRead = false,
                    AddedDate = DateTime.Now,
                    Message = $"{currentUser?.Name} '{posts.SeoLink}' id li yeni bir ilan paylaştı.",
                    ReceiveUserId = adminUsers.ElementAt(0).Id,
                    SenderUserId = currentUser.Id,
                    RedirectLink = $"ilan?s={posts.SeoLink}&approvalPurpose=true",
                    RelatedElementId = posts.SeoLink,
                    NotificationType = NotificationType.PostSharing
                });
                posts.Status = PostStatus.PendingApproval;
            }
            else
            {
                posts.Status = PostStatus.Active;
            }
            _context.Add(posts);
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
                    posts.Status = PostStatus.PendingApproval;
                    if (!isAdmin)
                    {
                        var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
                        await _context.Notification.AddAsync(new Notification()
                        {
                            IsRead = false,
                            AddedDate = DateTime.Now,
                            Message = $"{currentUser?.Name} '{posts.SeoLink}' ilanında güncelleme yaptı",
                            ReceiveUserId = adminUsers.ElementAt(0).Id,
                            SenderUserId = currentUser.Id,
                            RedirectLink = $"ilan?s={posts.SeoLink}&approvalPurpose=true",
                            RelatedElementId = posts.SeoLink,
                            NotificationType = NotificationType.PostSharing
                        });
                    }

                    _context.Update(posts);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostsExists(posts.Id))
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
                var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
                await _context.Notification.AddAsync(new Notification()
                {
                    IsRead = false,
                    AddedDate = DateTime.Now,
                    Message = $"{currentUser?.Name} '{post.SeoLink}' ilanını arşivden yayına almak istiyor",
                    ReceiveUserId = adminUsers.ElementAt(0).Id,
                    SenderUserId = currentUser.Id,
                    RedirectLink = $"ilan?s={post.SeoLink}&approvalPurpose=true",
                    RelatedElementId = post.SeoLink,
                    NotificationType = NotificationType.PostSharing
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> PostDelete(Users currentUser, Guid postId)
        {
            if (_context.Posts == null)
            {
                throw new JsonException("Kayıt Bulunamadı.");
            }

            var posts = await _context.Posts.FirstOrDefaultAsync(n => n.Guid == postId);

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

        public async Task<string> NotificationRead(int notifId)
        {
            var notif = _context.Notification.Find(notifId);
            if (notif == null)
            {
                throw new BadRequestException($"There is no notification with this id : {notifId}");
            }
            notif.IsRead = true;

            _context.Notification.Update(notif);

            await _context.SaveChangesAsync();

            return notif.RedirectLink;
        }

        public IEnumerable GetPostTypes() 
        {
            return _context.PostTypes;
        }
        public IEnumerable GetInstruments()
        {
            return _context.Instruments;
        }

        private bool PostsExists(int id)
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
            string seoLink = post.Title.Replace(" ", "-").ToLower();
            seoLink += new Random().Next(0, 9999999) + new Random().Next(0, 9999);
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
