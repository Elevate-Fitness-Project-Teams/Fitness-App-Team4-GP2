using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using FCEService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FCEService.Infrastructure.Persistence
{
    public class FceUnitofWork : IFceUnitOfWork
    {
        private readonly FCEDbContext _db;
        private IDbContextTransaction? _transaction;

     
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

      
        public async Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken ct = default)
        {
            _transaction = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                await action(ct);
                await _db.SaveChangesAsync(ct);
                await _transaction.CommitAsync(ct);
            }
            catch
            {
                await _transaction.RollbackAsync(ct);
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task CreateSavepointAsync(string name, CancellationToken ct = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException(
                    "No active transaction. Call ExecuteInTransactionAsync first.");

            await _transaction.CreateSavepointAsync(name, ct);
        }

        public async Task RollbackToSavepointAsync(string name, CancellationToken ct = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("No active transaction.");

            await _transaction.RollbackToSavepointAsync(name, ct);
        }
    }
}
