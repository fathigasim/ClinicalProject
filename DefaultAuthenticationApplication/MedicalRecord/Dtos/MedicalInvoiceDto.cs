using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Dtos
{
    public class MedicalInvoiceDto
    {
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
    
        public DateTime MedicalRecordDate { get; set; }
        public List<InvoicePrescriptionItemsDto> PrescriptionItems { get; set; }
    }
}
