using FCEService.Domain.Entities;

namespace FCEService.Domain.Interfaces
{
    public interface IFceUnitOfWork
    {
        IFitnessPlanConfigRepository FitnessPlanConfigs { get; }


        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct = default);

       

        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
