


using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class AppointmentRepository :Repository<Appointment> ,IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context):base(context)
        {
            
        }

        public async Task<IReadOnlyList<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId, CancellationToken ct = default)
        {
            return await _dbSet.Where(a => a.DoctorId == doctorId).ToListAsync(ct);
        }

        public async Task<bool> IsDoctorAppointmentsBusy(Guid doctorId, DateTime appointmentDate, CancellationToken ct = default)
        {
            return await _dbSet
                .AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate.Hour == appointmentDate.Hour, ct);
        }
    }
}
