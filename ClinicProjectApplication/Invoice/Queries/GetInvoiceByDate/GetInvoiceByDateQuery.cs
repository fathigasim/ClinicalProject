
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectDomain.Common.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetInvoiceByDate
{
    public record GetInvoiceByDateQuery :IRequest<Result<PagedResult<InvoicesDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public DateTime date { get; set; }
    }
}
