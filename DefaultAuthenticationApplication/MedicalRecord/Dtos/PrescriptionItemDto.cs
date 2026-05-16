using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Dtos
{
    public class PrescriptionItemDto
    {
        public string Dosage { get; set; }
        public string MedicationName { get; set; }
        public int Frequency { get; set; }
        public DateTime Duration { get; set; }
         
    }
}
