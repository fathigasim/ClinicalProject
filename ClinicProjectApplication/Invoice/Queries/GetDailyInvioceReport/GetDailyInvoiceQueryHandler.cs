using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetDailyInvioceReport
{
    public class GetDailyInvoiceQueryHandler : IRequestHandler<GetDailyInvoiceQuery, Result<List<DailyInvoiceDto>>>
    {
        private readonly IInvoiceService _invoiceService;
        public GetDailyInvoiceQueryHandler(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        public async Task<Result<List<DailyInvoiceDto>>> Handle(GetDailyInvoiceQuery request, CancellationToken cancellationToken)
        {
            var dailyInvoiceReport = await _invoiceService.GetDailyInvoices(cancellationToken);
            if (dailyInvoiceReport != null)
            {
                return Result<List<DailyInvoiceDto>>.Success(dailyInvoiceReport);
            }
            else
            {
                return Result<List<DailyInvoiceDto>>.Failure("No daily invoices found.");
            }
        }
    } 
}
