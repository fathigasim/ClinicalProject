using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Prescriptions : BaseEntity, IAuditableEntity
    {
        private Prescriptions(Guid medicalRecordId)
        {
            MedicalRecordId=medicalRecordId;
        }
        public Guid MedicalRecordId { get;private set; }
       
        private MedicalRecords _MedicalRecord ;
        public MedicalRecords MedicalRecord => _MedicalRecord;
        private readonly List<PrescriptionItems> _PrescriptionItems= new();

        public IReadOnlyCollection<PrescriptionItems> PrescriptionItems => _PrescriptionItems;

        public static Prescriptions CreatePrescription(Guid medicalRecordId)
        {
          return    new Prescriptions(medicalRecordId);
        }
        public void AddItem(string medicationName, string dosage, int frequency, int durationDays)
        {
            var item =ClinicProjectDomain.Entities.PrescriptionItems.Create(this.Id,medicationName, dosage, frequency, durationDays);
            _PrescriptionItems.Add(item);
        }
    }
}
