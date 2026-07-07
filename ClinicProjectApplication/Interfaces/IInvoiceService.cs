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
        Task<InvoicesDto> GetPatientInfoByInvoiceId(Guid invoiceId, CancellationToken cancellationToken);
        Task<List<MedicalInvoiceDto>> PatientsMedicalRecordInvoices();
        Task<List<MedicalInvoiceDto>> PatientMedicalRecordInvoicesByAppointmentNumber(string AppointmentNumber);
        Task<List<InvoicesDto>> GetAllInvoices(CancellationToken cancellationToken);
        Task<List<InvoicesDto>> GetLatestInvoices(CancellationToken cancellationToken);

        Task<List<MonthlyInvoiceDto>> GetMonthlyInvoices(CancellationToken cancellationToken);
        Task<List<WeeklyInvoiceDto>> GetWeeklyInvoices(CancellationToken cancellationToken);
        Task<List<DailyInvoiceDto>> GetDailyInvoices(CancellationToken cancellationToken);
    }
}
