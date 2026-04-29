using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Prescriptions
    {
        public Guid Id { get; set; }
        public Guid MedicalRecordId { get; set; }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        public DateTime CreatedAt { get; set; }
        public MedicalRecords MedicalRecord { get; set; }

        public ICollection<PrescriptionItems> PrescriptionItems { get; set; } = new List<PrescriptionItems>();

    }
}
