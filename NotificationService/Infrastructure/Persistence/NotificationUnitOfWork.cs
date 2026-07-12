using NotificationService.Domain.Interfaces;
using NotificationService.Infrastructure.Persistence.Repositories;
using System.Collections.Concurrent;

namespace NotificationService.Infrastructure.Persistence
{
    public sealed class NotificationUnitOfWork(NotificationDbContext dbContext) : INotificationUnitOfWork
    {
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public IGenericRepository<T> GetRepository<T>() where T : class
            => (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new GenericRepository<T>(dbContext));

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => dbContext.SaveChangesAsync(ct);
    }
}