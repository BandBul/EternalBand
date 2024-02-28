using EternalBAND.Common;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;
using EternalBAND.DomainObjects.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;
using X.PagedList;

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
                await _context.Posts
                .Where(p => p.Status == Common.PostStatus.Active)
                .Include(n => n.PostTypes)
                .Include(n => n.Instruments)
                .ToListAsync();
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

        public async Task<IPagedList<Posts>> Posts(int pageId, string typeShort, int cityId, string instrument)
        {
            var filters = new List<Func<Posts, bool>>();

            var currentPosts2 = _context.Posts.Where(p => p.Status == PostStatus.Active).Include(n => n.PostTypes).Include(n => n.Instruments);

            if(typeShort != "0" && typeShort != "")
            {
                filters.Add(n => n.PostTypes.TypeShort.ToString().Contains(typeShort));
            }

            if (instrument != "0" && instrument != "")
            {
                filters.Add(n => n.Instruments.InstrumentShort == instrument);
            }

            if (cityId != 0)
            {
                filters.Add(n => n.CityId == cityId);
            }

            Func<Posts, bool> predicate = post => filters.All(filter => filter(post));

            return await currentPosts2.Where(predicate)
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
                .Where(n => n.PostTypes.TypeShort == postTypeName.ToString() && n.Status == PostStatus.Active )
                .OrderByDescending(s => s.AddedDate)
                .ToPagedListAsync(1, Constants.PageSizeForElements);
        }

        public async Task<IPagedList<Posts>> NewPosts()
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
