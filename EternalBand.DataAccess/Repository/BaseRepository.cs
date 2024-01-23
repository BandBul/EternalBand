using EternalBAND.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace EternalBAND.DataAccess.Repository
{
    //TODO remove all usage of ApplicationDbContext and instead use BaseRepository.
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _dbContext;
        public BaseRepository(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        protected async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected void Commit()
        {
            _dbContext.SaveChanges();
        }
    }

    public class BaseRepository<TEntity> : BaseRepository
        where TEntity : class
    {
        protected readonly DbSet<TEntity> _dbSet;

        public DbSet<TEntity> Context => _dbSet;
        public BaseRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _dbSet = _dbContext.Set<TEntity>();
        }

        public bool IsExist(Func<TEntity, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IEnumerable<TEntity> Where(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate);
        }

        #region DML

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            Commit();
        }

        public void DeleteById(object id)
        {
            var element = _dbSet.Find(id);
            _dbSet.Remove(element);
            Commit();
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await CommitAsync();
        }

        #endregion
    }
}
