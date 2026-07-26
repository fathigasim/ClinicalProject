using ClinicProjectApplication.Common;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Queries
{
    public class DailyPaymentSalesQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<DailyPaymentSalesQuery, Result<decimal>>
    {
        public async Task<Result<decimal>> Handle(DailyPaymentSalesQuery request, CancellationToken cancellationToken)
        {
          var dailyPaymentSales= await paymentRepository.DailyPaymentSales(cancellationToken);
            if (dailyPaymentSales.Equals(0))
            {
                return Result<decimal>.Success(0);
            }
            return Result<decimal>.Success(dailyPaymentSales);
        }
    }
}
