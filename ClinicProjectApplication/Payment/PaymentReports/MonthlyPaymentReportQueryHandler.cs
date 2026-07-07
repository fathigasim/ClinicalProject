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
    public class MonthlyPaymentReportQueryHandler:IRequestHandler<MonthlyPaymentReportQuery,List<MonthlyPaymentsDto>>
    {
      private readonly  IPaymentReportService _paymentReportService;
        public MonthlyPaymentReportQueryHandler(IPaymentReportService paymentReportService)
        {
            _paymentReportService = paymentReportService;
        }
        public async Task<List<MonthlyPaymentsDto>> Handle(MonthlyPaymentReportQuery request, CancellationToken cancellationToken)
        {
           var monthlyPayments= await _paymentReportService.GetMonthlyPayments(cancellationToken);
            return monthlyPayments;
        }

      
    }
}
