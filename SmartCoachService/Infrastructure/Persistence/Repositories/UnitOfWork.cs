using Microsoft.EntityFrameworkCore.Storage;
using SmartCoachService.Domain.Contracts;

namespace SmartCoachService.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartCoachDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(SmartCoachDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var EntityType = typeof(T);
            if (_repositories.TryGetValue(EntityType, out var repository))
                return (IGenericRepository<T>)repository;
            var NewRepo = new GenericRepository<T>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _dbContext.Database.BeginTransactionAsync();
        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
