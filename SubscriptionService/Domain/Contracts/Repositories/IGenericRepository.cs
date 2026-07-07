using System.Linq.Expressions;

namespace SubscriptionService.Domain.Contracts.Repositories
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task SaveChangesAsync();
    }
}
