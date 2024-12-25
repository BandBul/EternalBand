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
using X.PagedList.EF;
using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Hosting;
using EternalBAND.Api.Helpers;
using EternalBAND.Api.Extensions;
using System.Reflection.Metadata;

namespace EternalBAND.Api.Services
{
    public class AdminService
    {
        private readonly BroadCastingManager _broadcastingManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AdminService(ApplicationDbContext context, UserManager<Users> userManager, IWebHostEnvironment environment, BroadCastingManager broadcastingManager)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _broadcastingManager = broadcastingManager;
        }

        public async Task<IPagedList<Blogs>> BlogsIndex(int pId = 1)
        {
            if (_context.Blogs == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.Blogs'  is null.");
            }

            return await _context.Blogs.OrderByDescending(n => n.Id).ToPagedListAsync(pId, 10);
        }

        public async Task BlogsCreate(Blogs blog, List<IFormFile>? images)
        {
            var source = blog.SeoLink == null || StrConvert.IsInjectionString(blog.SeoLink) ? blog.Title : blog.SeoLink;

            await BlogAddSeoLink(blog, source);
            await BlogPhotoAdd(blog, images);
            blog.AddedDate = DateTime.Now;

            _context.Add(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<Blogs> BlogsEditInitial(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                throw new NotFoundException();
            }

            var blogs = await _context.Blogs.FindAsync(id);
            if (blogs == null)
            {
                throw new NotFoundException();
            }

            return blogs;
        }

        public async Task BlogsEdit(Blogs editedBlog, List<IFormFile?> images, List<string>? deletedFilesIndex)
        {
            try
            {
                var existingBlog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(n => n.Id == editedBlog.Id);

                UpdatePhotos(editedBlog, images, deletedFilesIndex, existingBlog.AllPhotos);

                editedBlog.AddedDate = existingBlog.AddedDate;

                _context.Update(editedBlog);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogsExists(editedBlog.Id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<string> BlogsDeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                throw new JsonException("Kayıt bulunamadı.");
            }

            var blogs = await _context.Blogs.FindAsync(id);
            if (blogs != null)
            {
                _context.Blogs.Remove(blogs);
            }

            await _context.SaveChangesAsync();
            return "Blog yazısı silindi.";
        }

        public async Task<IPagedList<Contacts>> ContactsIndex(int pId = 1)
        {
            if (_context.Blogs == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }

            return await _context.Contacts.OrderByDescending(n => n.Id).ToPagedListAsync(pId, 10);
        }

        public async Task<Contacts> ContactsDetails(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                throw new NotFoundException();
            }

            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts == null)
            {
                throw new NotFoundException();
            }

            return contacts;
        }

        public async Task<Contacts> GetContacts(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                throw new NotFoundException();
            }

            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts == null)
            {
                throw new NotFoundException();
            }

            return contacts;
        }

        public async Task ContactEdit(int id, Contacts contacts)
        {
            if (id != contacts.Id)
            {
                throw new NotFoundException();
            }

            try
            {
                _context.Update(contacts);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactsExists(contacts.Id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task ContactsDeleteConfirmed(int? id)
        {
            if (_context.Contacts == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.Contacts'  is null.");
            }

            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts != null)
            {
                _context.Contacts.Remove(contacts);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IPagedList<PostTypes>> PostTypesIndex(int pId = 1)
        {
            if (_context.PostTypes == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.PostTypes'  is null.");
            }

            return await _context.PostTypes.OrderByDescending(n => n.Id).ToPagedListAsync(pId, 10);
        }

        public async Task PostTypesCreate(PostTypes postTypes)
        {
            postTypes.Type = StrConvert.TRToEnDeleteAllSpacesAndToLower(postTypes.FilterText);
            postTypes.Active = true;
            postTypes.AddedDate = DateTime.Now;
            _context.Add(postTypes);
            await _context.SaveChangesAsync();
        }

        public async Task<PostTypes> GetPostType(int? id)
        {
            if (id == null || _context.PostTypes == null)
            {
                throw new NotFoundException();
            }

            var postType = await _context.PostTypes.FindAsync(id);
            if (postType == null)
            {
                throw new NotFoundException();
            }

            return postType;
        }

        public async Task PostTypesEdit(int id, PostTypes postTypes)
        {
            if (id != postTypes.Id)
            {
                throw new NotFoundException();
            }

            try
            {
                var post = await _context.PostTypes.FirstOrDefaultAsync(n => n.Id == id);
                post.Active = postTypes.Active;
                post.FilterText = postTypes.FilterText;
                post.Type = StrConvert.TRToEnDeleteAllSpacesAndToLower(postTypes.FilterText);
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostTypesExists(postTypes.Id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task PostTypesDeleteConfirmed(int? id)
        {
            if (_context.PostTypes == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.PostTypes'  is null.");
            }

            var postTypes = await _context.PostTypes.FindAsync(id);
            if (postTypes != null)
            {
                _context.PostTypes.Remove(postTypes);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IPagedList<Instruments>> InstrumentsIndex(int pId = 1)
        {
            if (_context.Instruments == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.Instruments'  is null.");
            }

            return await _context.Instruments.OrderByDescending(n => n.Id).ToPagedListAsync(pId, 10);
        }

        public async Task InstrumentsCreate(Instruments instruments)
        {
            instruments.InstrumentShort = StrConvert.TRToEnDeleteAllSpacesAndToLower(instruments.Instrument);
            instruments.IsActive = true;
            _context.Add(instruments);
            await _context.SaveChangesAsync();
        }

        public async Task<Instruments> GetInstrument(int? id)
        {
            if (id == null || _context.Instruments == null)
            {
                throw new NotFoundException();
            }

            var instruments = await _context.Instruments.FindAsync(id);
            if (instruments == null)
            {
                throw new NotFoundException();
            }

            return instruments;
        }

        public async Task InstrumentsEdit(int id, Instruments instruments)
        {
            if (id != instruments.Id)
            {
                throw new NotFoundException();
            }

            try
            {
                instruments.InstrumentShort = StrConvert.TRToEnDeleteAllSpacesAndToLower(instruments.Instrument);
                _context.Update(instruments);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstrumentsExists(instruments.Id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task InstrumentsDeleteConfirmed(int id)
        {
            if (_context.Instruments == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.Instruments'  is null.");
            }

            var instruments = await _context.Instruments.FindAsync(id);
            if (instruments != null)
            {
                _context.Instruments.Remove(instruments);
            }

            await _context.SaveChangesAsync();
        }

        public async Task ApprovePost(string postSeoLink, Users currentUser)
        {
            var post = await ChangePostTypeAndGetPost(postSeoLink, PostStatus.Active);
            var message = $"<strong>'{post.Title}'</strong> başlıklı ilanınız yayına alınmıştır.";
            await InternalApproveRejectPost(post, currentUser, message);
        }

        public async Task RejectPost(string postSeoLink, Users currentUser)
        {
            var post = await ChangePostTypeAndGetPost(postSeoLink, PostStatus.Rejected);
            var message = $"<strong>'{post.Title}'</strong> başlıklı ilanınız, ilan kurallarına uymadığı için onaylanmamıştır. Lütfen ilan kurallarına uygun olacak şekilde tekrar düzenleyiniz.";
            await InternalApproveRejectPost(post, currentUser, message);
        }

        private async Task<Posts> ChangePostTypeAndGetPost(string postSeoLink, PostStatus status)
        {
            var post = _context.Posts.Where(p => p.SeoLink.Equals(postSeoLink)).FirstOrDefault();
            post.Status = status;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task InternalApproveRejectPost(Posts post, Users currentUser, string message)
        {
            // TODO remove admin notification
            var allNotifOnAdmin = _context.Notification.Where(not => not.RelatedElementId.Equals(post.SeoLink) && not.IsRead == false).ToList();
            allNotifOnAdmin.ForEach(n =>
            {
                n.IsRead = true;
            });

            _context.UpdateRange(allNotifOnAdmin);
            await _context.SaveChangesAsync();


            await _broadcastingManager.CreateCustomNotification(
            currentUser.Id,
                post.AddedByUserId,
                post,
                message
                );
        }

        public async Task<Posts?> Post(string seoLink)
        {
            return await _context.Posts.Include(n => n.PostTypes).Include(n => n.Instruments)
            .FirstOrDefaultAsync(n => n.SeoLink == seoLink);
        }

        public async Task<IPagedList<Posts>> GetFilteredPosts(Expression<Func<Posts,bool>> predicate)
        {
            return await _context.Posts
                .Where(predicate)
                .OrderByDescending(s => s.AddedDate)
                .ToPagedListAsync(1, 10) ;
        }

        private bool InstrumentsExists(int? id)
        {
            return (_context.Instruments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool PostTypesExists(int? id)
        {
            return (_context.PostTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool ContactsExists(int id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool BlogsExists(int id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private async Task Physical_Delete_Photos(List<string>? filesToDelete)
        {
            if (filesToDelete != null && filesToDelete.Count > 0)
            {
                foreach (var fileName in filesToDelete)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
        }

        private async Task BlogPhotoAdd(Blogs blogs, List<IFormFile?> images)
        {
            if (images != null)
            {
                foreach (var image in images)
                {
                    var absoluteFilePath = ImageHelper.GetGeneratedAbsoluteBlogImagePath(blogs.Id, image.FileName);
                    string fulldirectoryPath = Path.Combine(_environment.WebRootPath, Path.GetDirectoryName(absoluteFilePath));

                    // Ensure the directory exists
                    if (!Directory.Exists(fulldirectoryPath))
                    {
                        Directory.CreateDirectory(fulldirectoryPath);
                    }

                    var stream = new FileStream(Path.Combine(_environment.WebRootPath, absoluteFilePath), FileMode.Create);

                    try
                    {
                        
                        await image.CopyToAsync(stream);
                        if (blogs.PhotoPath == null)
                        {
                            blogs.PhotoPath = absoluteFilePath;
                            continue;
                        }

                        if (blogs.PhotoPath2 == null)
                        {
                            blogs.PhotoPath2 = absoluteFilePath;
                            continue;
                        }

                        if (blogs.PhotoPath3 == null)
                        {
                            blogs.PhotoPath3 = absoluteFilePath;
                            continue;
                        }

                        if (blogs.PhotoPath4 == null)
                        {
                            blogs.PhotoPath4 = absoluteFilePath;
                            continue;
                        }

                        if (blogs.PhotoPath5 == null)
                        {
                            blogs.PhotoPath5 = absoluteFilePath;
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        // TODO manage exception + add log
                        throw;
                    }

                    finally
                    {
                        stream?.Dispose();
                    }
                }
            }
        }

        private async Task BlogAddSeoLink(Blogs blog, string source)
        {
            while (true)
            {
                var seo = SeoLinkHelper.CreateSeo(source);
                if (!await _context.Blogs.AnyAsync(n => n.SeoLink.ToUpper() == seo.ToUpper()))
                {
                    blog.SeoLink = seo;
                    break;
                }
            }
        }

        private async Task UpdatePhotos(Blogs blog, List<IFormFile>? images, List<string>? deletedFilesIndex, List<string> exPhotos)
        {
            blog.SetPhoto(exPhotos);

            if (!deletedFilesIndex.IsNullOrEmpty())
            {
                var filesToDelete = new List<string?>();
                deletedFilesIndex.ForEach(index =>
                {
                    switch (int.Parse(index))
                    {
                        case 1:
                            filesToDelete.Add(blog.PhotoPath);
                            blog.PhotoPath = null;
                            break;
                        case 2:
                            filesToDelete.Add(blog.PhotoPath2);
                            blog.PhotoPath2 = null;
                            break;
                        case 3:
                            filesToDelete.Add(blog.PhotoPath3);
                            blog.PhotoPath3 = null;
                            break;
                        case 4:
                            filesToDelete.Add(blog.PhotoPath4);
                            blog.PhotoPath4 = null;
                            break;
                        case 5:
                            filesToDelete.Add(blog.PhotoPath5);
                            blog.PhotoPath5 = null;
                            break;
                    }
                });
                ImageHelper.DeletePhotos(_environment.WebRootPath, filesToDelete);
            }

            await BlogPhotoAdd(blog, images);
        }
    }
}
