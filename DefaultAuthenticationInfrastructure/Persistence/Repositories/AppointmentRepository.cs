


using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
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

        public async Task<bool> IsSlotOccupied(Guid doctorId, DateTime requestedDate, int durationMinutes, CancellationToken ct = default)
        {
            // Define the start and end of the requested appointment
            var requestedStart = requestedDate;
            var requestedEnd = requestedDate.AddMinutes(durationMinutes);
//            How the Overlap Logic Works
//The formula(StartA < EndB) && (EndA > StartB) is the standard way to detect if two time periods overlap.
            // Check if any existing appointment overlaps with this window
            return await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                               a.status != AppointmentStatus.Cancelled && // Ignore cancelled ones
                               requestedStart < a.AppointmentDate.AddMinutes(durationMinutes) &&
                               a.AppointmentDate < requestedEnd, ct);
        }
    }
}
