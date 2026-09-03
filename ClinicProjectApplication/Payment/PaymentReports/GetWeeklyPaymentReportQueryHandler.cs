using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.PaymentReports
{
    public class GetWeeklyPaymentReportQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetWeeklyPaymentReportQuery,List< WeeklyPaymentSummaryDto>>
    {
        public async Task<List<WeeklyPaymentSummaryDto>> Handle(GetWeeklyPaymentReportQuery request, CancellationToken cancellationToken)
        {
        var weeklySummery=     await paymentRepository.PaymentsWeeklyTotal(cancellationToken);

            var weeklySummeryDto = weeklySummery.Select(p => new WeeklyPaymentSummaryDto(p.dayOfWeek, p.total)).ToList();
            return weeklySummeryDto;
        }
    }
}
