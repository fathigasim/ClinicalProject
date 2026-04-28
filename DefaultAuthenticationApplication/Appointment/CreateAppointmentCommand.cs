
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments
{
    public record CreateAppointmentCommand
        (Guid PatiendId,Guid DoctorId, DateTime AppointmentDate, string Notes) :IRequest<Guid>,ITransactionalRequest;

   
   

   

   
   
 

}
