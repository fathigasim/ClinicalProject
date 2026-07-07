using ClinicProjectApplication.PrescriptionsItems.Dtos;
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Prescription.Dtos
{
    public record PrescriptionsDto
    {
        
        public Guid MedicalRecordId { get; set; }
        public DateTime CreatedAt { get; set; }
    

        public ICollection<PrescriptionItemsDto> PrescriptionItemsDto { get; set; } = new List<PrescriptionItemsDto>();

    }
}
