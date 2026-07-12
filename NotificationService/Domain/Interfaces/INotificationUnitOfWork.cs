namespace NotificationService.Domain.Interfaces
{
  
    public interface INotificationUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}