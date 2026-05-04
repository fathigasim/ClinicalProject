using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Command.DoctorWeeklySchedule
{
    public record CreateWeeklyScheduleCommand (Guid DoctorId,DayOfWeek DayOfWeek, TimeOnly startTime, TimeOnly endTime) 
        :IRequest<Result<string>>, ITransactionalRequest
    {
    }
}
