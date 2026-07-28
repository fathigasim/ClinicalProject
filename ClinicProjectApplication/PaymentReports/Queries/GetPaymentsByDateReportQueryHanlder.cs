using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.PaymentReports.Dto;
using ClinicProjectDomain.Common.Pagination;
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
    public class GetPaymentsByDateReportQueryHanlder(IPaymentRepository paymentRepository,IMapper mapper) : IRequestHandler<GetPaymentsByDateReportQuery, Result<PagedResult<PaymentReportDto>>>
    {
        public async Task<Result<PagedResult<PaymentReportDto>>> Handle(GetPaymentsByDateReportQuery request, CancellationToken cancellationToken)
        {

         var payments=   await paymentRepository.PaymentsByDate(request.date,request.page,request.pageSize,cancellationToken);
                 if(payments is null ||payments.Items.Count()==0)
            {
                return Result<PagedResult<PaymentReportDto>>.Failure("No payments made on this day");
            }
          var paymentsDto=  mapper.Map<List<PaymentReportDto>>(payments.Items);

            return Result<PagedResult<PaymentReportDto>>.Success(new PagedResult<PaymentReportDto>
            {
                Items=paymentsDto,
                Page=request.page,
                PageSize=request.pageSize,
                TotalCount= payments.TotalCount
            });
        }
    }
}
