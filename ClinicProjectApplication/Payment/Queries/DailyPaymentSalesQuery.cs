using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Queries
{
    public record DailyPaymentSalesQuery:IRequest<Result<decimal>>;
   
}
