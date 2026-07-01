using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.PaymentReports
{
    public class DailyPaymentReportQueryHandler:IRequestHandler<DailyPaymentReportQuery, List<DailyPaymentsDto>>
    {
        private readonly IPaymentReportService _paymentReportService;
        public DailyPaymentReportQueryHandler(IPaymentReportService paymentReportService)
        {
            _paymentReportService = paymentReportService;
        }
        public async Task<List<DailyPaymentsDto>> Handle(DailyPaymentReportQuery request, CancellationToken cancellationToken)
        {
            var dailyPayments = await _paymentReportService.GetDailyPayments(cancellationToken);

            return dailyPayments;
        }
    }
}
