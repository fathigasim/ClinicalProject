using AutoMapper;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Queries
{
    public class GetMonthlyPaymentsTotalsQueryHandler(IPaymentRepository paymentRepository,IMapper mapper) : IRequestHandler<GetMonthlyPaymentsTotalsQuery,List<MonthlyTotalPaymentsDto>>
    {
        public async Task <List<MonthlyTotalPaymentsDto>> Handle(GetMonthlyPaymentsTotalsQuery request, CancellationToken cancellationToken)
        {
       var result=      await  paymentRepository.PaymentsMonthlyTotal(cancellationToken);
              var monthlyTotals= result.Select(p=>new MonthlyTotalPaymentsDto(p.Month,p.TotalAmount)).ToList();
             return monthlyTotals;
        }
    }
}
