
using ClinicProjectDomain.Entities;
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
        Task <WeeklySchedule?> DoctorWeeklySchedule(Guid doctorId,DayOfWeek dayOfWeek,CancellationToken cancellationToken);
        Task<List<Doctor>?> DoctorsTodaySchedule(CancellationToken cancellationToken);
    }
}
