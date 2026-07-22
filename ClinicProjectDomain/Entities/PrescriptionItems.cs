using ClinicProjectDomain.Exceptions;
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
        public string MedicationName { get;private set; }
        public string Dosage { get;private set; }
        public int Frequency { get;private set; }
        public int Duration { get;private set; }
        private Prescriptions _Prescription;
        public Prescriptions Prescription => _Prescription;
        public static PrescriptionItems Create(Guid prescriptionId, string medicationName, string dosage, int frequency, int durationDays)
        {
            if (string.IsNullOrWhiteSpace(medicationName))
                throw new DomainException("Medication name is required.");
            if (frequency <= 0)
                throw new DomainException("Frequency must be positive.");
            if (durationDays <= 0)
                throw new DomainException("Duration must be positive.");

            return new PrescriptionItems(prescriptionId, medicationName, dosage, frequency, durationDays);
        }
    }
}
