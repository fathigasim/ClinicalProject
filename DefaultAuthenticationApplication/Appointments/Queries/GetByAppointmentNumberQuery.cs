using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Queries
{
    public record GetByAppointmentNumberQuery :IRequest<Result<List<AppointmentDto>>>;
    
}
