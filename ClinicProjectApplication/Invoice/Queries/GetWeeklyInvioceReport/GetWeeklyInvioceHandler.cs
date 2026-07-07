using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetWeeklyInvioceReport
{
    public class GetWeeklyInvioceHandler : IRequestHandler<GetWeeklyInvioce, Result<List<WeeklyInvoiceDto>>>
    {
        private readonly IInvoiceService _invoiceService;
        public GetWeeklyInvioceHandler(IInvoiceService invoiceService)
        {
            _invoiceService= invoiceService;
        }
        public async Task<Result<List<WeeklyInvoiceDto>>> Handle(GetWeeklyInvioce request, CancellationToken cancellationToken)
        {
            var weeklyInvoices = await _invoiceService.GetWeeklyInvoices(cancellationToken);
            if (weeklyInvoices == null)
            {
                return Result<List<WeeklyInvoiceDto>>.Failure("No weekly invoices found");
            }
            return Result<List<WeeklyInvoiceDto>>.Success(weeklyInvoices);
        }
    }
}
