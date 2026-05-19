using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class MedicalRecords :BaseEntity, IAuditableEntity
    {
       
        public string MedicalRecordNumber { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = default!;
        public string Diagnosis { get; set; } = default!;
        public ICollection<Prescriptions> Prescriptions{ get; set; } = new List<Prescriptions>();
  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}
