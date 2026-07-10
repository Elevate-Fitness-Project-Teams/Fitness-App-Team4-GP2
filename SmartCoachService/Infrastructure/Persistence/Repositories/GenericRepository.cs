using Microsoft.EntityFrameworkCore;
using SmartCoachService.Domain.Contracts;
using System.Linq.Expressions;

namespace SmartCoachService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SmartCoachDbContext _smartCoachDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SmartCoachDbContext smartCoachDbContext)
        {
            _smartCoachDbContext = smartCoachDbContext;
            _dbSet = _smartCoachDbContext.Set<T>();
        }
        public void Add(T entity) => _dbSet.Add(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);


        public IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null)
        {
            if (expression is not null)
                return _dbSet.Where(expression);
            return _dbSet;
        }

        public Task<T?> GetOneAsync(Expression<Func<T, bool>> expression) => _dbSet.Where(expression).FirstOrDefaultAsync();


        public async Task SaveChangesAsync() => await _smartCoachDbContext.SaveChangesAsync();

        public void Update(T entity) => _dbSet.Update(entity);

    }
}
