using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    // Application/Common/Interfaces/ITransactionalRequest.cs
    /// <summary>
    /// Commands that modify state implement this interface to get
    /// automatic transaction wrapping from TransactionBehavior.
    /// Queries deliberately omit it — reads never need a write transaction.
    /// </summary>
    public interface ITransactionalRequest { }
}
