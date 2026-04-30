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
    public record CreateWeeklyScheduleCommand (Guid DoctorId,DayOfWeek DayOfWeek, DateTime StartTime, DateTime EndTime) 
        :IRequest<Result<string>>, ITransactionalRequest
    {
    }
}
