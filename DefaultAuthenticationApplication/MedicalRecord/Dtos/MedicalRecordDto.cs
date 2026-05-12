using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Dtos
{
    public record MedicalRecordDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public string Diagnosis { get; set; }
        public ICollection<PrescriptionsDto> Prescriptions { get; set; } = new List<PrescriptionsDto>();
        public string Notes { get; set; }
        
      public DateTime CreatedAt { get; set; }
    }
}
