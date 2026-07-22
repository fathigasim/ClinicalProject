


using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Extensions;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class AppointmentRepository :Repository<Appointment> ,IAppointmentRepository
    {
        private readonly IReadDbContext _readDbContext;
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context, IReadDbContext readDbContext) :base(context)
        {
            _readDbContext = readDbContext;
            _context = context;
        }
        public async Task<PagedResult<Appointment>> GetTodaysAppointmentsAsync(int page,int pageSize ,CancellationToken cancellationToken)
        {
            var dayOfWeek = DateTime.Now.DayOfWeek;
            var todayDate = DateOnly.FromDateTime(DateTime.Now.Date);
            return await _readDbContext.ReadSet<Appointment>()
                //.Where(p => p.DayOfWeek == dayOfWeek &&p.CreatedAt.Date>=DateTime.Now.Date)
                .Where(p => p.IsBooked == false
                && p.AppointmentDate==todayDate)
                .Take(100)
                .OrderByDescending(p=>p.AppointmentNumber)
                .ToPagedAsync(page, pageSize, cancellationToken);
        }

        public async Task<IReadOnlyList<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId, DateOnly appointmentDate, CancellationToken cancellationToken)
        {
           
            return await  _readDbContext.ReadSet<Appointment>()
                .Where(a => a.DoctorId == doctorId &&
                            a.Status != AppointmentStatus.Cancelled &&
                            a.AppointmentDate == appointmentDate) 
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Appointment>> GetDatedAppointmentsByDoctorIdAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
        {
          

            return await
                //_readDbContext.ReadSet<Appointment>()
                _context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                            a.Status != AppointmentStatus.Cancelled &&

                            a.AppointmentDate== date)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Appointment>> GetOverlappingAppointmentsAsync(
    Guid doctorId,
    DateOnly date,
    TimeOnly requestedStartTime,
    CancellationToken cancellationToken)
        {
            // 1. Pre-calculate the requested window in C#
            TimeOnly requestedEndTime = requestedStartTime.AddMinutes(30);

            // 2. Run the overlap query
            return await _readDbContext.ReadSet<Appointment>()
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.Status != AppointmentStatus.Cancelled &&
                    a.AppointmentDate == date &&

                    // Overlap Math logic:
                    // An existing appointment overlaps with our requested time if:
                    // Existing Start is BEFORE requested End AND Existing End is AFTER requested Start.
                    a.StartTime < requestedEndTime &&
                    a.StartTime.AddMinutes(30) > requestedStartTime
                )
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetByAppointmentNumberAsync(string appointmentNo, CancellationToken cancellationToken)
        {


            return await _context.Appointments.Include(p => p.Patient)
                .Where(a => a.AppointmentNumber.Contains(appointmentNo))
                // a.status != AppointmentStatus.Cancelled &&

                .FirstOrDefaultAsync(cancellationToken);
        }

        //    public async Task<AppointmentSearchResultDto> GetByAppointmentNumberAsync(
        //string appointmentNo,
        //CancellationToken cancellationToken)
        //    {
        //        return await _readDbContext.ReadSet<Appointment>()
        //            .Where(a => a.AppointmentNumber.Contains(appointmentNo))
        //            // Explicitly Join the Patient table using the PatientId FK
        //            .Join(
        //                _readDbContext.ReadSet<Patient>(), // Target table
        //                appt => appt.PatientId,            // FK on Appointment
        //                patient => patient.Id,             // PK on Patient
        //                (appt, patient) => new AppointmentSearchResultDto // The projected result
        //                {
        //                    AppointmentId = appt.Id,
        //                    AppointmentNumber = appt.AppointmentNumber,
        //                    AppointmentDate = appt.AppointmentDate,
        //                    StartTime = appt.StartTime,
        //                    Notes = appt.Notes,

        //                    // Now you can safely map the Patient details!
        //                    PatientId = patient.Id,
        //                    PatientName = string.Concat( patient.FirstName,patient.LastName) // Or whatever property you have

        //                }
        //            )
        //            .FirstOrDefaultAsync(cancellationToken);
        //    }

        public async Task<List<Appointment>?> GetListOfNotInvoicedAppointmentsAsync(CancellationToken cancellationToken)
        {


            return await _readDbContext.ReadSet<Appointment>()
                //.Include(p => p.Invoice)
                .Where(a => a.Invoice==null)
                // a.status != AppointmentStatus.Cancelled &&
                .ToListAsync(cancellationToken);
           
        }

    }
}
