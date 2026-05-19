using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class PrescriptionItems :BaseEntity, IAuditableEntity
    {
       
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public int Frequency { get; set; }
        public DateTime Duration { get; set; }
        public Prescriptions Prescription { get; set; }
    }
}
