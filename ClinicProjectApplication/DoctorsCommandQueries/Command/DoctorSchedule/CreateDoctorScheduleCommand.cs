using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule
{
    public record CreateDoctorScheduleCommand  
        (Guid? DoctorId, DateOnly ScheduleDate, TimeOnly StartTime, TimeOnly EndTime) : IRequest<Result<string>>, ITransactionalRequest;
   
}
