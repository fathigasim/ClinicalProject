using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Services
{
    public class PaymentReportService : IPaymentReportService
    {
        private readonly IReadDbContext _readDbContext;
        public PaymentReportService(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }
        public async Task<List<MonthlyPaymentsDto?>> GetMonthlyPayments(CancellationToken cancellationToken)
        {
            var payments = await _readDbContext.ReadSet<Payments>()
     .GroupBy(p => p.PaidAt.Month)
     .Select(g => new
     {
         Month = g.Key,
         Amount = g.Sum(x => x.Amount) / 100m
     })
     .ToListAsync(cancellationToken);
            //using System.Globalization;

            //CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)

            //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Month)

//            GetAbbreviatedMonthName(11) // "Nov"
//GetMonthName(11)           // "November"
            var result = payments
                .Select(x => new MonthlyPaymentsDto(
                    new DateTime(2000, x.Month, 1).ToString("MMM"),
                    x.Amount))
                .ToList();
            return result;
        }

        public async Task<List<DailyPaymentsDto?>> GetDailyPayments(CancellationToken cancellationToken)
        {
       var dailyPayments=    await _readDbContext.ReadSet<Payments>().GroupBy(p => p.PaidAt.Day)
             .Select(p => new DailyPaymentsDto(p.Select(p=>p.PaidAt.Date.ToString("dd/MM/yy")).FirstOrDefault(),p.Key.ToString(),p.Sum(p => p.Amount) / 100m))
             .ToListAsync(cancellationToken);
             
             

            return dailyPayments;
        }
    }
}
