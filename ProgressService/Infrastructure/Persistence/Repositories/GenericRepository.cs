using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;
using System.Linq.Expressions;

namespace ProgressService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : BaseEntity
    {
        private readonly ProgressDbContext _progressDbContext;
        private DbSet<T> _dbSet;

        public GenericRepository(ProgressDbContext progressDbContext)
        {
            _progressDbContext = progressDbContext;
            _dbSet = _progressDbContext.Set<T>();
        }
        public void Add(T entity) => _dbSet.Add(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);
  
        public IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null)
        {
            if(expression is not null)
                return _dbSet.Where(expression);
            return _dbSet;
        }
        

        public async Task<T?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);

        public Task<T?> GetOneAsync(Expression<Func<T, bool>> expression) => _dbSet.Where(expression).FirstOrDefaultAsync();


        //public async Task SaveChangesAsync() => await _progressDbContext.SaveChangesAsync();

        public void SaveIncluded(T entity, params string[] IncludeProperties)
        {

            var local = _dbSet.Local.FirstOrDefault(entry => entry.Id == entity.Id);

            EntityEntry entry;

            if (local is null)
                entry = _progressDbContext.Entry(entity);
            else
                entry = _progressDbContext.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity.Id == entity.Id)!;
           
            entry.State = EntityState.Modified;

            foreach (var includeProperty in IncludeProperties)
            {
                entry.Reference(includeProperty).IsModified = true;
            }
        }

        public void Update(T entity) => _dbSet.Update(entity);
        
    }
}
