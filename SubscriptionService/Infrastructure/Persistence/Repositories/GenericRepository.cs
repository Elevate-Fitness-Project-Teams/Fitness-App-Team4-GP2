using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Contracts.Repositories;
using SubscriptionService.Domain.Entities;
using System.Linq.Expressions;

namespace SubscriptionService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SubscriptionDbContext _subscriptionDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SubscriptionDbContext subscriptionDbContext)
        {
            _subscriptionDbContext = subscriptionDbContext;
            _dbSet = _subscriptionDbContext.Set<T>();
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


        public async Task SaveChangesAsync() => await _subscriptionDbContext.SaveChangesAsync();

        public void Update(T entity) => _dbSet.Update(entity);

    }
}
