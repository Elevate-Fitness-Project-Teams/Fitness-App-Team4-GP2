using FCEService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace FCEService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T>(FCEDbContext dbContext) : IGenericRepository<T> where T : class
    {
        protected readonly FCEDbContext _dbContext = dbContext;
        protected readonly DbSet<T> dbSet = dbContext.Set<T>();

        public Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
            => dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct);

        public Task<T?> FirstOrderedAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            CancellationToken ct = default)
            => orderBy(dbSet.AsNoTracking().Where(predicate)).FirstOrDefaultAsync(ct);

        public async Task<IReadOnlyList<T>> WhereAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
            => await dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => dbSet.AsNoTracking().AnyAsync(predicate, ct);

        public IQueryable<T> Query() => dbSet.AsNoTracking();

        
        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await dbSet.AddAsync(entity, ct);

        public void Update(T entity) => dbSet.Update(entity);

        public void SaveInclude(T entity, params string[] includedProperties)
        {
            var entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            foreach (var propertyName in includedProperties)
            {
                entry.Property(propertyName).IsModified = true;
            }
        }
    }
}