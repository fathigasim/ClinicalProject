
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IInvoiceRepository:IRepository<Invoices>
    {

        Task<List<Invoices>> GetAll(CancellationToken cancellationToken);
        Task<PagedResult<Invoices>> GetInvoiceByDate(int page, int pageSize, DateTime date, CancellationToken cancellationToken);
        Task<Invoices?> GetInvoiceByInvoiceNumberAsync(string invoiceNumber, CancellationToken ct = default);
        

    }
}
