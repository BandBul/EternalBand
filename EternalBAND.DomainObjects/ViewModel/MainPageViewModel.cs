using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace EternalBAND.DomainObjects.ViewModel
{
    public class MainPageViewModel
    {
        public IPagedList<Posts> NewPosts { get; set; }    
        public IPagedList<Blogs> NewBlogs { get; set; }    
    }
}
