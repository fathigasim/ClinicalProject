using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PrescriptionsItems.Dtos
{
    public record PrescriptionItemsDto
    {
   
        public Guid PrescriptionId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public int Frequency { get; set; }
        public DateTime Duration { get; set; }
     
    }
}
