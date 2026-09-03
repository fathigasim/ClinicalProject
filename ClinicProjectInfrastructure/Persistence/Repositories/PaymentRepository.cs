using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectDomain.Models;
using ClinicProjectInfrastructure.Extensions;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class PaymentRepository :Repository<Payments>,IPaymentRepository
    {
        private readonly IReadDbContext _readDbContext;
        public PaymentRepository(AppDbContext context,IReadDbContext readDbContext):base(context)
        {
            _readDbContext = readDbContext;
        }

        public async Task<decimal> DailyPaymentSales(CancellationToken cancellationToken)
        {
          return   await _readDbContext.ReadSet<Payments>().SumAsync(p=>p.Amount,cancellationToken);
        }

        public async Task<PagedResult<Payments?>> PaymentsByDate(DateTime date,int page,int pageSize, CancellationToken cancellationToken)
        {
            return await _readDbContext.ReadSet<Payments>().Include(p=>p.Invoice).Where(p=>p.PaidAt.Date==date.Date)
                .ToPagedAsync(page,pageSize,cancellationToken);
        }

        public async Task<List<Payments?>> PaymentsListByDate(DateTime date, CancellationToken cancellationToken)
        {
            return await _readDbContext.ReadSet<Payments>().Include(p => p.Invoice).Where(p => p.PaidAt.Date == date.Date)
                .ToListAsync(cancellationToken);
        }



        public async Task<List<MonthlyPaymentSummary>> PaymentsMonthlyTotal( CancellationToken cancellationToken)
        {
            var previousMonth = DateTime.UtcNow.AddMonths(-1).Month;
            var currentYear = DateTime.UtcNow.Year;
            return await _readDbContext.ReadSet<Payments>()
                .Where(p =>  p.PaidAt.Year == currentYear)
                .GroupBy(p => p.PaidAt.Month)
                .Select(p => new MonthlyPaymentSummary(p.Key,p.Sum(p=>p.Amount)))
                .ToListAsync(cancellationToken);
        }


        public async Task<List<WeeklyPaymentSummary>> PaymentsWeeklyTotal(CancellationToken cancellationToken)
        {
            //var currentMonth = DateTime.UtcNow.Month;
            //var currentYear = DateTime.UtcNow.Year;
            var previousWeek = DateTime.UtcNow.AddDays(-7);
          var raw=   await _readDbContext.ReadSet<Payments>()
                .Where(p => p.PaidAt>=previousWeek)
                
                .Select(p => new { p.PaidAt,p.Amount})
                .ToListAsync(cancellationToken);

            var result = raw
    .GroupBy(p => p.PaidAt.DayOfWeek)
    .Select(g => new WeeklyPaymentSummary(g.Key.ToString(), g.Sum(x => x.Amount)))
    .ToList();

            return result;
        }

    }
}
