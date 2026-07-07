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
    public record UpdateAppointmentCommand:IRequest<Result<string>>,ITransactionalRequest
    {
        public string  AppointmentNo { get; set; }
        
        public TimeOnly StartTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

      
    }
}
