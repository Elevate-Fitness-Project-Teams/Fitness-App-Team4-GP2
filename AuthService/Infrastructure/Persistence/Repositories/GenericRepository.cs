using AuthService.BuildingBlocks.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Linq.Expressions;

namespace AuthService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AuthDbContext _context;

        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AuthDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public IQueryable<T> GetAllAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (include is not null)
            {
                query = include(query);
            }

            return query;
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

    }
}
