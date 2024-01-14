using EternalBAND.Common;

namespace EternalBAND.Models
{
    public class PostByStatusModel
    {
        public List<Posts> Posts { get; set; }
        public PostStatus Status { get; set; }
    }
}
