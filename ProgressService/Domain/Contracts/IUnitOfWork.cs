using Microsoft.EntityFrameworkCore.Storage;
using ProgressService.Domain.Entities;

namespace ProgressService.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();

        IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity;
    }
}
