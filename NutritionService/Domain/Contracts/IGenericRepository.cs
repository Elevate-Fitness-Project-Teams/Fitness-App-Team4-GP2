using System.Linq.Expressions;

namespace NutritionService.Domain.Contracts
{
    public interface IGenericRepository<T>
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(int id);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        Task SaveChangesAsync();

    }
}
