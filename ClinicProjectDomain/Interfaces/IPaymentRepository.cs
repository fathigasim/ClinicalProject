using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IPaymentRepository :IRepository<Payments>
    {

        public   Task<decimal> DailyPaymentSales (CancellationToken cancellationToken);

       public Task<PagedResult<Payments?>> PaymentsByDate(DateTime date, int page, int pageSize, CancellationToken cancellationToken);
        Task<List<Payments?>> PaymentsListByDate(DateTime date, CancellationToken cancellationToken);
         Task<List<MonthlyPaymentSummary>> PaymentsMonthlyTotal( CancellationToken cancellationToken);

        Task<List<WeeklyPaymentSummary>> PaymentsWeeklyTotal(CancellationToken cancellationToken);
    }
}
