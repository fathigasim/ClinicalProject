
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
        Task <WeeklySchedule?> DoctorWeeklySchedule(Guid doctorId,DayOfWeek dayofWeek,CancellationToken cancellationToken);
        Task<WeeklySchedule?> DoctorSchedule(Guid doctorId, DateOnly sheduleDate, CancellationToken cancellationToken);
       
        Task<List<Doctor>?> DoctorsTodaySchedule(CancellationToken cancellationToken);
        Task<WeeklySchedule?> DoctorScheduleDate(Guid doctorId, DateOnly date, CancellationToken cancellationToken);
    }
}
