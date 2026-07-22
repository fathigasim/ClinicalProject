using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IWeeklyScheduleRepository :IRepository<DoctorSchedule>
    {
        //Task AddDoctoryWeeklyScheduleAsync(WeeklySchedule weeklySchedule, CancellationToken ct);
        Task<bool> IsDoctorScheduledToday(Guid doctorId, DateOnly scheduleDate, DayOfWeek dayofweek, CancellationToken ct);
        Task<bool> HasOverlappingSchedule(Guid? doctorId,
        DateOnly scheduleTime,
        DayOfWeek day,
        TimeOnly start,
        TimeOnly end,
        CancellationToken ct);
        Task<List<DoctorSchedule>> DoctorsScheduleDays(CancellationToken ct);
    }

    
}
