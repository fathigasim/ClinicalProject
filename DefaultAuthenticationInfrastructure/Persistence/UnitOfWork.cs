using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence
{


    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _currentTransaction;
        
        public UnitOfWork(AppDbContext db, IUserRepository users)
        {
            _db = db;
            
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _db.Database.BeginTransactionAsync(ct);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _db.SaveChangesAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is null)
                throw new InvalidOperationException("No active transaction to commit.");

            try
            {
                await _db.SaveChangesAsync(ct);
                await _currentTransaction.CommitAsync(ct);
            }
            catch
            {
                await RollbackTransactionAsync(ct);
                throw; // rethrows original exception (DbUpdateException → 409)
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }


        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is null) return; // guard first

            try
            {
                await _currentTransaction.RollbackAsync(ct);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> work,
            CancellationToken ct = default)
            => throw new NotSupportedException(
                "Use BeginTransactionAsync / CommitTransactionAsync instead.");

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async ValueTask DisposeAsync()
            => await DisposeTransactionAsync();
    }
}
