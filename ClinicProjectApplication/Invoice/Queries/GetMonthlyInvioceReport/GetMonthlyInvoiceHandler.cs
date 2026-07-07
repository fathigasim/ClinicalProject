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

namespace ClinicProjectApplication.Invoice.Queries.GetMonthlyInvioceReport
{
    public class GetMonthlyInvoiceHandler : IRequestHandler<GetMonthlyInvoice, Result<List<MonthlyInvoiceDto>>>
    {
      private readonly   IInvoiceService _invoiceService;
        public GetMonthlyInvoiceHandler(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<Result<List<MonthlyInvoiceDto>>> Handle(GetMonthlyInvoice request, CancellationToken cancellationToken)
        {
            var monthlyInvoices= await _invoiceService.GetMonthlyInvoices(cancellationToken);
            if (monthlyInvoices == null) {
                return Result<List<MonthlyInvoiceDto>>.Failure("No available data");
            }
            return Result<List<MonthlyInvoiceDto>>.Success(monthlyInvoices);
        }
    }
}
