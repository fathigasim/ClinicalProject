using AutoMapper;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PaymentReports.Queries
{
    public class GetPaymentsByDatePdfReportQueryHandler(IPaymentRepository paymentRepository,IMapper mapper) : IRequestHandler<GetPaymentsByDatePdfReportQuery, List<PaymentDto>>
    {
        public async Task<List<PaymentDto>> Handle(GetPaymentsByDatePdfReportQuery request, CancellationToken cancellationToken)
        {
            var payments= await paymentRepository.PaymentsListByDate(request.date, cancellationToken);
            if(payments is  null || payments.Count==0)
            {
                return new List<PaymentDto>();
            }
            var paymentsDto= mapper.Map<List<PaymentDto>>(payments);
            return paymentsDto;
        }
    }
}
