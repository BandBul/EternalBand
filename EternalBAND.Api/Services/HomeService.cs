using EternalBAND.Api.Extensions;
using EternalBAND.Common;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;
using X.PagedList.EF;

namespace EternalBAND.Api.Services
{
    public class HomeService
    {
        private readonly ApplicationDbContext _context;
        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blogs>> Blogs(int pageId, string blogText = "")
        {
            if (string.IsNullOrEmpty(blogText))
            {
                return await _context.Blogs.ToPagedListAsync(pageId, 10);
            }
            return await _context.Blogs.Where(n => n.Title.Contains(blogText)).ToPagedListAsync(pageId, 10);
        }

        public async Task<Blogs> Blog(string seoLink)
        {
            return await _context.Blogs.FirstOrDefaultAsync(n => n.SeoLink == seoLink);
        }

        public async Task<IPagedList<Posts>> GetNewPosts()
        {
            var currentPosts =
                _context.Posts
                .Where(p => p.Status == Common.PostStatus.Active)
                .Include(n => n.PostTypes)
                .Include(n => n.Instruments);
            return await currentPosts
                .OrderByDescending(s => s.AddedDate)
                .ToPagedListAsync(1, Constants.PageSizeForElements);
        }

        public async Task<IPagedList<Blogs>> GetNewBlogs()
        {
            return await _context.Blogs
                .OrderByDescending(s => s.AddedDate)
                .ToPagedListAsync(1, Constants.PageSizeForBlogs);
        }

        public async Task<MainPageViewModel> GetMainPageModel()
        {
            return new MainPageViewModel
            {
                NewBlogs = await GetNewBlogs(),
                NewPosts = await GetNewPosts()
            };
        }

        public async Task<IPagedList<Posts>> FilterPostsByType(int pageId, string type, int cityId, string instrument)
        {
            var query = _context.Posts.Where(p => p.Status == PostStatus.Active);

            var filters = new List<Expression<Func<Posts, bool>>>();

            if(type.IsValidForFilter())
            {
                query = query.Where(n => n.PostTypes.Type.Contains(type));
            }

            if (instrument.IsValidForFilter())
            {
                query = query.Where(n => n.Instruments.InstrumentShort == instrument);
            }

            if (cityId != 0)
            {
                query = query.Where(n => n.CityId == cityId);
            }


            return await query
                    .Include(n => n.PostTypes)
                    .Include(n => n.Instruments)
                    .OrderByDescending(s => s.AddedDate)
                    .ToPagedListAsync(pageId, Constants.PageSizeForElements);
        }

        public async Task<Posts?> Post(string seoLink)
        {
            return await _context.Posts.Include(n => n.PostTypes).Include(n => n.Instruments)
            .FirstOrDefaultAsync(n => n.SeoLink == seoLink);
        }

        public async Task<Contacts> ContactsCreate(Contacts contacts)
        {
            contacts.AddedDate = DateTime.Now;

            _context.Add(contacts);
            await _context.SaveChangesAsync();

            return contacts;
        }

        public async Task<IPagedList<Posts>> PostsByPostTypeAsync(PostTypeName postTypeName)
        {
            return await _context.Posts
                .Include(n => n.PostTypes)
                .Include(n => n.Instruments)
                .Where(n => n.PostTypes.Type == postTypeName.ToString() && n.Status == PostStatus.Active )
                .OrderByDescending(s => s.AddedDate)
                .ToPagedListAsync(1, Constants.PageSizeForElements);
        }

        public async Task<IPagedList<Posts>> GetRecentPosts()
        {
            return await _context.Posts
                .Include(n => n.PostTypes)
                .Include(n => n.Instruments)
                .Where(n => n.Status == PostStatus.Active)
                .OrderByDescending(s => s.AddedDate)
                .ToPagedListAsync(1, Constants.PageSizeForElements);
        }
    }
}
