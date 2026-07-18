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

            
        public PrescriptionItems(Guid prescriptionId, string medicationName, string dosage, int frequency, int duration)
        {
            PrescriptionId = prescriptionId;
            MedicationName = medicationName;
            Dosage = dosage;
            Duration = duration;
            Frequency = frequency;
        }
        public Guid PrescriptionId { get;private set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public int Frequency { get; set; }
        public int Duration { get; set; }
        private Prescriptions _Prescription;
        public Prescriptions Prescription => _Prescription;
        public static PrescriptionItems Create(Guid prescriptionId, string medicationName, string dosage, int frequency, int durationDays)
        {
            if (string.IsNullOrWhiteSpace(medicationName))
                throw new ArgumentException("Medication name is required.", nameof(medicationName));
            if (frequency <= 0)
                throw new ArgumentException("Frequency must be positive.", nameof(frequency));
            if (durationDays <= 0)
                throw new ArgumentException("Duration must be positive.", nameof(durationDays));

            return new PrescriptionItems(prescriptionId, medicationName, dosage, frequency, durationDays);
        }
    }
}
