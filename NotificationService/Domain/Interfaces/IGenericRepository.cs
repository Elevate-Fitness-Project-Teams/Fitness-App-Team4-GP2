using System.Linq.Expressions;

namespace NotificationService.Domain.Interfaces
{
    
    public interface IGenericRepository<T> where T : class
    {
       
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<T?> FirstOrderedAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            CancellationToken ct = default);
        Task<IReadOnlyList<T>> WhereAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
        IQueryable<T> Query();
        Task AddAsync(T entity, CancellationToken ct = default);
        void Update(T entity);
        void SaveInclude(T entity, params string[] includedProperties);
    }
}