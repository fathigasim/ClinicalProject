using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule
{
    public class DeleteDoctorScheduleCommandHandler(IDoctorScheduleRepository repository) : IRequestHandler<DeleteDoctorScheduleCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            var doctorShcedule=  await repository.DoctorsScheduleById(request.Id,cancellationToken);

            if (doctorShcedule == null)
            {
                return Result<string>.Failure("No doctor schedule found");
            }
           
            repository.Delete(doctorShcedule);
            return Result<string>.Success($"Doctor {doctorShcedule.Doctor.FirstName} schedule deleted successfully");

        }
    }
}
