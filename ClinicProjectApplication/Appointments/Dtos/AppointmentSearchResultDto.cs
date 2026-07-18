using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.Dtos
{
    public class AppointmentSearchResultDto
    {
        public Guid AppointmentId { get; set; }
        public string AppointmentNumber { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public string Notes { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName
        {
            get; set;


        }
    }
}
