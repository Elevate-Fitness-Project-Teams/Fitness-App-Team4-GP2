using System.Linq.Expressions;

namespace ProgressService.Domain.Contracts
{
    public interface IGenericRepository<T, TKey>
    {

        IQueryable<T> GetAll(Expression<Func<T,bool>>?expression = null);
        Task<T?> GetOneAsync(Expression<Func<T,bool>> expression);
        Task<T?> GetByIdAsync(TKey id);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        void SaveIncluded(T entity , params string[] IncludeProperties);
        //Task SaveChangesAsync();
    }
}
