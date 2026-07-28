using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Extensions;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
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
    }
}
