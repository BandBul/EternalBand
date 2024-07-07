namespace EternalBAND.DomainObjects.ViewModel
{
    public class PostViewModel
    {
        public X.PagedList.IPagedList<Posts> Posts { get; set; }
        public PostFilterContracts PostFilterContracts { get; set; }
    }
}
