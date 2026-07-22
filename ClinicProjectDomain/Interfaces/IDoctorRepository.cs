
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IDoctorRepository: IRepository<Doctor>
    {
        Task<List<Doctor>> GetListedDoctorsAsync( CancellationToken ct);
        Task<PagedResult<Doctor>> GetAllDoctorsAsync(int page, int pageSize,CancellationToken ct);
        Task <DoctorSchedule?> DoctorWeeklySchedule(Guid doctorId,DayOfWeek dayofWeek,CancellationToken cancellationToken);
        Task<DoctorSchedule?> DoctorSchedule(Guid doctorId, DateOnly sheduleDate, CancellationToken cancellationToken);
       
        Task<List<Doctor>?> DoctorsTodaySchedule(CancellationToken cancellationToken);
        Task<DoctorSchedule?> DoctorScheduleDate(Guid doctorId, DateOnly date, CancellationToken cancellationToken);
    }
}
