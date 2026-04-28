using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class MedicalRecords
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
