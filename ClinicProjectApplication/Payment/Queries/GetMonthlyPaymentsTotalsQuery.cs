using ClinicProjectApplication.Payment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Queries
{
    public record GetMonthlyPaymentsTotalsQuery:IRequest<List<MonthlyTotalPaymentsDto>>;
   
}
