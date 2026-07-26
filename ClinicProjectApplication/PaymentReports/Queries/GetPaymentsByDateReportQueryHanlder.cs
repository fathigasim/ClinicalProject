using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.PaymentReports.Dto;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PaymentReports.Queries
{
    public class GetPaymentsByDateReportQueryHanlder(IPaymentRepository paymentRepository,IMapper mapper) : IRequestHandler<GetPaymentsByDateReportQuery, Result<List<PaymentReportDto>>>
    {
        public async Task<Result<List<PaymentReportDto>>> Handle(GetPaymentsByDateReportQuery request, CancellationToken cancellationToken)
        {

         var payments=   await paymentRepository.PaymentsByDate(request.date,cancellationToken);
                 if(payments == null)
            {
                return Result<List<PaymentReportDto>>.Failure("No payments made on this day");
            }
          var paymentsDto=  mapper.Map<List<PaymentReportDto>>(payments);

            return Result<List<PaymentReportDto>>.Success(paymentsDto);
        }
    }
}
