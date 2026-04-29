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
        Task<IReadOnlyList<Appointment>> GetAppointmentsByDoctorIdAsync(Guid doctorId, CancellationToken ct = default);
        Task<bool> IsDoctorAppointmentsBusy(Guid doctorId, DateTime appointmentDate, CancellationToken ct = default);
    }
}
