using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Dtos
{
    public record NotInvoicedAppointmentDto(Guid id,string appointmentNumber)
    {
    }
}
