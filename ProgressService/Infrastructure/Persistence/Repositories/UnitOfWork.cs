using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProgressService.Domain.Contracts;
using ProgressService.Domain.Entities;

namespace ProgressService.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProgressDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(ProgressDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity
        {
            var EntityType = typeof(T);
            if (_repositories.TryGetValue(EntityType, out var repository))
                return (IGenericRepository<T, TKey>)repository;
            var NewRepo = new GenericRepository<T, TKey>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _dbContext.Database.BeginTransactionAsync();
        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
