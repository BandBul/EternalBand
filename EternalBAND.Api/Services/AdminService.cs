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

namespace EternalBAND.Api.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IWebHostEnvironment _environment;

        public AdminService(ApplicationDbContext context, UserManager<Users> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IPagedList<Blogs>> BlogsIndex(int pId = 1)
        {
            if (_context.Blogs == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.Blogs'  is null.");
            }

            return await _context.Blogs.OrderByDescending(n => n.Id).ToPagedListAsync(pId, 10);
        }

        public async Task BlogsCreate(Blogs blogs, List<IFormFile>? images)
        {
            await BlogPhotoAdd(blogs, images);
            blogs.SeoLink = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(blogs.Title) + "-" + new Random().Next(0, 999999);
            blogs.AddedDate = DateTime.Now;
            _context.Add(blogs);
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

        public async Task BlogsEdit(int id, Blogs blogs, List<IFormFile?> images)
        {
            if (id != blogs.Id)
            {
                throw new NotFoundException();
            }

            await BlogPhotoAdd(blogs, images);
            var blog = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(n => n.Id == blogs.Id);

            if (images.Count == 0)
            {
                blogs.PhotoPath = blog.PhotoPath;
                blogs.PhotoPath2 = blog.PhotoPath2;
                blogs.PhotoPath3 = blog.PhotoPath3;
                blogs.PhotoPath4 = blog.PhotoPath4;
                blogs.PhotoPath5 = blog.PhotoPath5;
            }

            try
            {
                blogs.AddedDate = blog.AddedDate;
                _context.Update(blogs);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogsExists(blogs.Id))
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
            postTypes.TypeShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(postTypes.Type);
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
                post.Type = postTypes.Type;
                post.TypeShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(postTypes.Type);
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

        public async Task<IPagedList<SystemInfo>> SystemInfoIndex(int pId = 1)
        {
            if (_context.SystemInfo == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.SystemInfo'  is null.");
            }

            return await _context.SystemInfo.OrderByDescending(n => n.Id).ToPagedListAsync(pId, 10);
        }

        public async Task SystemInfoCreate(SystemInfo systemInfo)
        {
            _context.Add(systemInfo);
            await _context.SaveChangesAsync();
        }

        public async Task<SystemInfo> GetSystemInfo(int? id)
        {
            if (id == null || _context.SystemInfo == null)
            {
                throw new NotFoundException();
            }

            var systemInfo = await _context.SystemInfo.FindAsync(id);
            if (systemInfo == null)
            {
                throw new NotFoundException();
            }

            return systemInfo;
        }

        public async Task SystemInfoEdit(int id, SystemInfo systemInfo, IFormFile? image)
        {
            if (id != systemInfo.Id)
            {
                throw new NotFoundException();
            }

            try
            {
                if (systemInfo.Type == "site-title")
                {
                    Business.AppSettings.SetAppSettingValue("SiteName", systemInfo.Value);
                }
                if (systemInfo.Type == "site-domain")
                {
                    Business.AppSettings.SetAppSettingValue("SiteDomain", systemInfo.Value);
                }

                if (systemInfo.Type == "site-logo" || systemInfo.Type == "site-favicon")
                {
                    if (image != null)
                    {
                        var photoName = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(image.FileName) + new PhotoName().GeneratePhotoName(new Random().Next(0, 10000000).ToString()) +
                                        new Random().Next(0, 1000) +
                                        System.IO.Path.GetExtension(image.FileName);
                        using (var stream =
                                new FileStream(
                                    Path.Combine(_environment.WebRootPath, "images/logo/", photoName),
                                    FileMode.Create))

                        {
                            await image.CopyToAsync(stream);
                        }

                        systemInfo.Value = "/images/logo/" + photoName;
                        if (systemInfo.Type == "site-logo")
                        {
                            Business.AppSettings.SetAppSettingValue("SiteLogo", systemInfo.Value);
                        }

                        if (systemInfo.Type == "site-favicon")
                        {
                            Business.AppSettings.SetAppSettingValue("SiteFavicon", systemInfo.Value);
                        }

                    }
                }

                _context.Update(systemInfo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemInfoExists(systemInfo.Id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task SystemInfoDeleteConfirmed(int id)
        {
            if (_context.SystemInfo == null)
            {
                throw new ProblemException("Entity set 'ApplicationDbContext.SystemInfo'  is null.");
            }

            var systemInfo = await _context.SystemInfo.FindAsync(id);
            if (systemInfo != null)
            {
                _context.SystemInfo.Remove(systemInfo);
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
            instruments.InstrumentShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(instruments.Instrument);
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
                instruments.InstrumentShort = new Business.StrConvert().TRToEnDeleteAllSpacesAndToLower(instruments.Instrument);
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
            var post = _context.Posts.Where(p => p.SeoLink.Equals(postSeoLink)).FirstOrDefault();
            post.Status = PostStatus.Active;
            _context.Update(post);

            // TODO getting all seolink of current post we need to add an check also received user is admin or not
            var allNotifOnAdmin = _context.Notification.Where(not => not.RelatedElementId.Equals(postSeoLink) && not.IsRead == false).ToList();
            allNotifOnAdmin.ForEach(n =>
            {
                n.IsRead = true;
            });

            _context.UpdateRange(allNotifOnAdmin);


            var adminUsers = await _userManager.GetUsersInRoleAsync(Constants.AdminRoleName);
            await _context.Notification.AddAsync(new Notification()
            {
                IsRead = false,
                AddedDate = DateTime.Now,
                Message = $"'{post.Title}' başlıklı ilanınız yayına alınmıştır",
                ReceiveUserId = post.AddedByUserId,
                SenderUserId = currentUser.Id,
                RedirectLink = $"ilan?s={post.SeoLink}",
                RelatedElementId = post.SeoLink,
                NotificationType = NotificationType.PostSharing
            });

            await _context.SaveChangesAsync();
        }

        private bool InstrumentsExists(int id)
        {
            return (_context.Instruments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool SystemInfoExists(int id)
        {
            return (_context.SystemInfo?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool PostTypesExists(int id)
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

        private async Task BlogPhotoAdd(Blogs blogs, List<IFormFile?> images)
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
                        if (blogs.PhotoPath == null)
                        {
                            blogs.PhotoPath = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (blogs.PhotoPath3 == null)
                        {
                            blogs.PhotoPath3 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (blogs.PhotoPath3 == null)
                        {
                            blogs.PhotoPath3 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (blogs.PhotoPath4 == null)
                        {
                            blogs.PhotoPath4 = "/images/ilan/" + photoName;
                            continue;
                        }

                        if (blogs.PhotoPath5 == null)
                        {
                            blogs.PhotoPath5 = "/images/ilan/" + photoName;
                            continue;
                        }
                    }
                }
            }
        }
    }
}
