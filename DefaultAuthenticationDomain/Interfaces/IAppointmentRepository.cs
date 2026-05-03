using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IAppointmentRepository:IRepository<Appointment>
    {
        Task<IReadOnlyList<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId, DayOfWeek dayOfWeek,CancellationToken cancellationToken);
        Task<Appointment?> GetByAppointmentNumberAsync(string appointmentNo, CancellationToken cancellationToken);
      //  Task<bool> IsDoctorAppointmentsBusy(Guid doctorId, DateOnly appointmentDate, CancellationToken ct = default);
      // Task<bool> IsSlotOccupied(Guid doctorId, DateTime requestedDate, int durationMinutes, CancellationToken ct = default);
    }
}
