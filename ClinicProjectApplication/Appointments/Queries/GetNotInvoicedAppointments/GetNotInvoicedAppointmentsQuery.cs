using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Queries.GetNotInvoicedAppointments
{
    public record  GetNotInvoicedAppointmentsQuery: IRequest<List<NotInvoicedAppointmentDto>>;
  
}
