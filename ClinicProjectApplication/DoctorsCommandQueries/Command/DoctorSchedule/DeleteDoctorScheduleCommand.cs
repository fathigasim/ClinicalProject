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
    public record DeleteDoctorScheduleCommand(Guid Id) :IRequest<Result<string>>,ITransactionalRequest;
   
}
