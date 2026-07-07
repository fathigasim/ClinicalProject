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
    public record CreateWeeklyScheduleCommand  
        :IRequest<Result<string>>, ITransactionalRequest
    {

        public Guid? DoctorId { get; init; }
        public DateOnly ScheduleDate { get; init; }
        public TimeOnly StartTime { get; init; }
        public TimeOnly EndTime { get; init; }
    }
}
