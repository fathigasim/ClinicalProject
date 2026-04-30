using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IWeeklyScheduleRepository :IRepository<WeeklySchedule>
    {
        //Task AddDoctoryWeeklyScheduleAsync(WeeklySchedule weeklySchedule, CancellationToken ct);
        Task<bool> IsDoctoryScheduledToday(Guid doctorId, DayOfWeek dayofweek, CancellationToken ct);
    }
}
