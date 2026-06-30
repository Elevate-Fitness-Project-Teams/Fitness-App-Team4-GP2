using System.Linq.Expressions;

namespace FCEService.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id, CancellationToken ct = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);

        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default);

        Task<T?> FirstOrderedAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            CancellationToken ct = default);

        Task<IReadOnlyList<T>> WhereAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default);

        Task AddAsync(T entity, CancellationToken ct = default);

   
        void SaveInclude(T entity, params string[] updatedProperties);

     
    }
}
