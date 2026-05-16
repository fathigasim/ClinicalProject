using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IInvoiceService
    {
        Task<List<MedicalInvoiceDto>> PatientsMedicalRecordInvoices();
        Task<List<MedicalInvoiceDto>> PatientMedicalRecordInvoicesByAppointmentNumber(string AppointmentNumber);
        Task<List<InvoicesDto>> GetAllInvoices(CancellationToken cancellationToken);
        Task<List<InvoicesDto>> GetLatestInvoices(CancellationToken cancellationToken);
    }
}
