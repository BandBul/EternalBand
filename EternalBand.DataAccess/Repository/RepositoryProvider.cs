
namespace EternalBAND.DataAccess.Repository
{
    public class RepositoryProvider
    {
        private readonly IEnumerable<BaseRepository> _repositories;
        private Dictionary<Type, BaseRepository> _typedRepos;
        public RepositoryProvider(IEnumerable<BaseRepository> repositories) 
        {
            _repositories = repositories;
            _typedRepos = new();
            foreach (var repository in _repositories)
            {
                var type = repository.GetType().GenericTypeArguments.FirstOrDefault();
                _typedRepos.Add(type, repository);
            }
        }
        // TODO try to convert this to the indexer
        public BaseRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            return _typedRepos[typeof(TEntity)] as BaseRepository<TEntity>;
        }

    }
}
