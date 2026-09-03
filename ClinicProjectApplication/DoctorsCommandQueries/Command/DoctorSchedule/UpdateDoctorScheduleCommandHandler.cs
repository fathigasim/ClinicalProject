using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule
{
    public class UpdateDoctorScheduleCommandHandler(IDoctorScheduleRepository repository) : IRequestHandler<UpdateDoctorScheduleCommand, Result<string>>,ITransactionalRequest
    {
        public async Task<Result<string>> Handle(UpdateDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
          var doctorSchedule=  await repository.DoctorsScheduleById(request.Id, cancellationToken);
            if (doctorSchedule == null)
            {

                return Result<string>.Failure("No Schedule has been found");
            }
            doctorSchedule.Update(request.Id,request.StartTime,request.EndTime,request.ScheduleDate);
            return Result<string>.Success("Schedule has been Updated");
            //return Result<string>.Success($"{doctorSchedule.Doctor.FirstName+" "+ doctorSchedule.Doctor.LastName} Schedule has been Updated");

        }
    }
}
