
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
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
        public InvoiceRepository(AppDbContext context):base (context)
        {
            
        }


        public async Task<IReadOnlyList<Invoices>> GetInvoicesByPatientIdAsync(Guid patientId, CancellationToken ct = default)
        {
            return await _dbSet.Include(a=>a.Appointment).ThenInclude(p=>p.Patient).Where(i => i.Appointment.Patient.Id == patientId).ToListAsync(ct);
        }

        //public async Task<bool> ISInvoiceIssued(Guid AppointmentId,CancellationToken ct = default)
        //{
        //    return await _dbSet.AnyAsync(i => i.AppointmentId == AppointmentId, ct);
        //}
    }
}
