using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetLatestInvoices
{
    public class GetLatestInvoicesQueryHandler : IRequestHandler<GetLatestInvoicesQuery,Result<List<InvoicesDto>>>
    {
        private readonly IInvoiceService _invoicesService;   
        public GetLatestInvoicesQueryHandler(IInvoiceService invoicesService)
        {
            _invoicesService = invoicesService;
        }
        public async Task<Result<List<InvoicesDto>>> Handle(GetLatestInvoicesQuery request, CancellationToken cancellationToken)
        {

            var latestInvoices = await _invoicesService.GetLatestInvoices(cancellationToken);

            if (latestInvoices == null ||latestInvoices.Count()<=0) {
              return  Result<List<InvoicesDto>>.Failure("No invoices were found");
            }
            return Result<List<InvoicesDto>>.Success(latestInvoices);
        }
    }
}
