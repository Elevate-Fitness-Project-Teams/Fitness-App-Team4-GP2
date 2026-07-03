using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NutritionService.Domain.Contracts;
using System.Linq.Expressions;

namespace NutritionService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
    
        private readonly NutritionDbContext _nutritionDbContext;
        private DbSet<T> _dbSet;


        public GenericRepository(NutritionDbContext nutritionDbContext)
        {
            _nutritionDbContext = nutritionDbContext;
            _dbSet = _nutritionDbContext.Set<T>();
        }
        public void Add(T entity) => _dbSet.Add(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? expression = null)
        {
            if (expression is not null)
                return _dbSet.Where(expression);
            return _dbSet;
        }


        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public Task<T?> GetOneAsync(Expression<Func<T, bool>> expression) => _dbSet.Where(expression).FirstOrDefaultAsync();

        public async Task SaveChangesAsync() => await _nutritionDbContext.SaveChangesAsync();


        public void Update(T entity) => _dbSet.Update(entity);

    }
}
