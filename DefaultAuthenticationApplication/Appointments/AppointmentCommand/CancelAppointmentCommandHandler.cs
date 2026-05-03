using ClinicProjectApplication.Common;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.AppointmentCommand
{
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand,Result<string>>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
           _appointmentRepository = appointmentRepository;   
        }
        public async Task <Result<string>>Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
          var appointment= await _appointmentRepository.GetByAppointmentNumberAsync(request.AppointmentNo,cancellationToken);
            if (appointment != null)
            {
                appointment.Cancel();

                return Result<string>.Success("Appointment canceled successfully");
            }
            return Result<string>.Failure("Appointment cannot be canceled");
        }
    }
}
