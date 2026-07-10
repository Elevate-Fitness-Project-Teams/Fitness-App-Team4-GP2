using Microsoft.EntityFrameworkCore.Storage;

namespace SmartCoachService.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();

        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
