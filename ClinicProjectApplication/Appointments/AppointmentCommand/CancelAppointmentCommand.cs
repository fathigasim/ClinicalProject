using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.AppointmentCommand
{
    public record CancelAppointmentCommand(string AppointmentNo) :IRequest<Result<string>>,ITransactionalRequest;


}
