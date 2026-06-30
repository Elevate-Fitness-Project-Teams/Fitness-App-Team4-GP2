using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace FCEService.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T>(FCEDbContext dbContext) where T : class
    {
        protected readonly FCEDbContext _dbContext = dbContext;
        protected readonly DbSet<T> dbSet = dbContext.Set<T>();


        public async Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
            => await dbSet.FindAsync([id], ct);

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
            => await dbSet.AsNoTracking().ToListAsync(ct);

        // TRACKED — caller intends to mutate + call Update()
        public Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
            => dbSet.FirstOrDefaultAsync(predicate, ct);

        // TRACKED + ordered — covers "latest by date" / "first matching X" patterns
        public Task<T?> FirstOrderedAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            CancellationToken ct = default)
            => orderBy(dbSet.Where(predicate)).FirstOrDefaultAsync(ct);

        // Read-only — for pure display/listing scenarios
        public async Task<IReadOnlyList<T>> WhereAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct = default)
            => await dbSet.AsNoTracking().Where(predicate).ToListAsync(ct);

        public async Task AddAsync(T entity, CancellationToken ct = default)
            => await dbSet.AddAsync(entity, ct);

        public void Update(T entity) => dbSet.Update(entity);


        public void SaveInclude(T entity, params string[] includedProperties)
        {

            var localEntity = dbSet.Local.FirstOrDefault((e) => e == entity);   

            EntityEntry<T> entry;

            if (localEntity == null)
            {
                dbSet.Attach(entity);
                entry = _dbContext.Entry(entity);
            }
            else
            {
                entry = _dbContext.Entry(localEntity);
                entry.CurrentValues.SetValues(entity);
            }

            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                property.IsModified = includedProperties.Contains(property.Metadata.Name);
            }
        }


    }
}
