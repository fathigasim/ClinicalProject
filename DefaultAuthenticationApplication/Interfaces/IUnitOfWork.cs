using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ClinicProjectApplication.Interfaces
{
    // Domain/Interfaces/IUnitOfWork.cs
    public interface IUnitOfWork
    {
    
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(
                       //IsolationLevel level = IsolationLevel.ReadCommitted,
                       CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
        bool HasActiveTransaction { get; }
    }
}
