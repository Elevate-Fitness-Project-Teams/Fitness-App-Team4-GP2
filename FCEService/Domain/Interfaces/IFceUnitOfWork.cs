using FCEService.Domain.Entities;

namespace FCEService.Domain.Interfaces
{
    public interface IFceUnitOfWork
    {
        IFitnessPlanConfigRepository FitnessPlanConfigs { get; }


        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
       
        //Task ExecuteInTransactionAsync(
        //    Func<CancellationToken, Task> action,
        //    CancellationToken ct = default);

        //Task CreateSavepointAsync(string name, CancellationToken ct = default);

        //Task RollbackToSavepointAsync(string name, CancellationToken ct = default);
    }
}
