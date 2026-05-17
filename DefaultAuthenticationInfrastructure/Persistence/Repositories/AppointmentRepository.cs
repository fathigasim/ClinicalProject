


using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class AppointmentRepository :Repository<Appointment> ,IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context):base(context)
        {
            
        }

        public async Task<IReadOnlyList<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId, DayOfWeek dayOfWeek, CancellationToken cancellationToken)
        {
           
            return await  _dbSet
                .Where(a => a.DoctorId == doctorId &&
                            a.status != AppointmentStatus.Cancelled &&
                            a.DayOfWeek == dayOfWeek) 
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetByAppointmentNumberAsync(string appointmentNo, CancellationToken cancellationToken)
        {
          

            return await _dbSet.Include(p=>p.Patient)
                .Where(a => a.AppointmentNumber.Contains(appointmentNo))
                           // a.status != AppointmentStatus.Cancelled &&
                            
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Appointment>?> GetListOfNotInvoicedAppointmentsAsync(CancellationToken cancellationToken)
        {


            return await _dbSet.Include(p => p.Invoices)
                .Where(a => a.Id!=a.Invoices.AppointmentId)
                // a.status != AppointmentStatus.Cancelled &&

                .ToListAsync(cancellationToken);
        }
        //public async Task<Appointment?> GetByAppointmentPatientAsync(string patientName, CancellationToken cancellationToken)
        //{


        //    return await _dbSet.Include(p=>p.Patient)
        //        .Where(a => a.Patient.FirstName )
        //        // a.status != AppointmentStatus.Cancelled &&

        //        .FirstOrDefaultAsync(cancellationToken);
        //}

        //public async Task<bool> IsDoctorAppointmentsBusy(Guid doctorId, DateTime appointmentDate, CancellationToken ct = default)
        //{
        //    return await _dbSet
        //        .AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate.Hour == appointmentDate.Hour, ct);
        //}

        //        public async Task<bool> IsSlotOccupied(Guid doctorId, DateTime requestedDate, int durationMinutes, CancellationToken ct = default)
        //        {
        //            // Define the start and end of the requested appointment
        //            var requestedStart = requestedDate;
        //            var requestedEnd = requestedDate.AddMinutes(durationMinutes);
        ////            How the Overlap Logic Works
        ////The formula(StartA < EndB) && (EndA > StartB) is the standard way to detect if two time periods overlap.
        //            // Check if any existing appointment overlaps with this window
        //            return await _context.Appointments
        //                .AnyAsync(a => a.DoctorId == doctorId &&
        //                               a.status != AppointmentStatus.Cancelled && // Ignore cancelled ones
        //                               requestedStart < a.AppointmentDate.AddMinutes(durationMinutes) &&
        //                               a.AppointmentDate < requestedEnd, ct);
        //        }
    }
}
