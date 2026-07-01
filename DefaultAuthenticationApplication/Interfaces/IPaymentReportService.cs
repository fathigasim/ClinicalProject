using ClinicProjectApplication.Payment.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IPaymentReportService
    {
        Task<List<MonthlyPaymentsDto?>> GetMonthlyPayments(CancellationToken cancellationToken);

        Task<List<DailyPaymentsDto?>> GetDailyPayments(CancellationToken cancellationToken);
    }
}
