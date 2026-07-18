
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
    public class InvoiceRepository: Repository<Invoices>, IInvoiceRepository
    {
        private readonly IReadDbContext _readDbContext;
        public InvoiceRepository(AppDbContext context, IReadDbContext readDbContext):base (context)
        {
            _readDbContext = readDbContext;
        }


        public async Task<IReadOnlyList<Invoices>> GetInvoicesByPatientIdAsync(Guid patientId, CancellationToken ct = default)
        {
            return await _dbSet.Include(a=>a.Appointment).ThenInclude(p=>p.Patient).Where(i => i.Appointment.Patient.Id == patientId).ToListAsync(ct);
        }
        public async Task<Invoices?> GetInvoiceByInvoiceNumberAsync(string invoiceNumber, CancellationToken ct = default)
        {
            return await _dbSet.Where(i => i.InvoiceNo == invoiceNumber).FirstOrDefaultAsync(ct);
        }

        public async Task<List<Invoices>> GetAll(CancellationToken cancellationToken)
        {
             return await _readDbContext.ReadSet<Invoices>().Include(a => a.Payment)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<Invoices>> GetInvoiceByDate(int page,int pageSize,DateTime date, CancellationToken cancellationToken)
        {
            return await _readDbContext.ReadSet<Invoices>()
                .Where(p => p.CreatedAt.Date.Equals(date))
                .ToPagedAsync(page, pageSize, cancellationToken);
        }
    }
}
