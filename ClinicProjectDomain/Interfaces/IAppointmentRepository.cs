using ClinicProjectDomain.Common.Pagination;
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
        Task<PagedResult<Appointment>> GetTodaysAppointmentsAsync(int page,int pageSize,CancellationToken cancellationToken);
        Task<IReadOnlyList<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId, DateOnly appointmentDate,CancellationToken cancellationToken);
        Task<Appointment?> GetByAppointmentNumberAsync(string appointmentNo, CancellationToken cancellationToken);
        Task<List<Appointment>?> GetListOfNotInvoicedAppointmentsAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<Appointment>> GetDatedAppointmentsByDoctorIdAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken);

        //  Task<bool> IsDoctorAppointmentsBusy(Guid doctorId, DateOnly appointmentDate, CancellationToken ct = default);
        // Task<bool> IsSlotOccupied(Guid doctorId, DateTime requestedDate, int durationMinutes, CancellationToken ct = default);
    }
}
