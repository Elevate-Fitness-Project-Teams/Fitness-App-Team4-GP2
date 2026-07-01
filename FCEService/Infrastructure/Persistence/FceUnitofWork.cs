using FCEService.Domain.Entities;
using FCEService.Domain.Interfaces;
using FCEService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FCEService.Infrastructure.Persistence
{
    public class FceUnitofWork(FCEDbContext db) : IFceUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();

        //private IDbContextTransaction? _transaction;

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var EntityType = typeof(T);
            if (_repositories.TryGetValue(EntityType, out var repository))
                return (IGenericRepository<T>)repository;
            var NewRepo = new GenericRepository<T>(db);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => db.SaveChangesAsync(ct);
    }
}
      
        //public async Task ExecuteInTransactionAsync(
        //    Func<CancellationToken, Task> action,
        //    CancellationToken ct = default)
        //{
        //    _transaction = await db.Database.BeginTransactionAsync(ct);

        //    try
        //    {
        //        await action(ct);
        //        await db.SaveChangesAsync(ct);
        //        await _transaction.CommitAsync(ct);
        //    }
        //    catch
        //    {
        //        await _transaction.RollbackAsync(ct);
        //        throw;
        //    }
        //    finally
        //    {
        //        await _transaction.DisposeAsync();
        //        _transaction = null;
        //    }
        //}

        //public async Task CreateSavepointAsync(string name, CancellationToken ct = default)
        //{
        //    if (_transaction is null)
        //        throw new InvalidOperationException(
        //            "No active transaction. Call ExecuteInTransactionAsync first.");

        //    await _transaction.CreateSavepointAsync(name, ct);
        //}

        //public async Task RollbackToSavepointAsync(string name, CancellationToken ct = default)
        //{
        //    if (_transaction is null)
        //        throw new InvalidOperationException("No active transaction.");

        //    await _transaction.RollbackToSavepointAsync(name, ct);
        //}


