using AutoMapper;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ClinicProjectInfrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IReadDbContext _readDbContext;
        private readonly IMapper _mapper;
        public InvoiceService(IInvoiceRepository invoiceRepository, IReadDbContext readDbContext, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _readDbContext = readDbContext;
            _mapper = mapper;
        }

        public async Task<List<InvoicesDto>> GetAllInvoices(CancellationToken cancellationToken)
        {
            var invoices = await _invoiceRepository.GetAll(cancellationToken);
              

            var invoicesDto = _mapper.Map<List<InvoicesDto>>(invoices);
            return invoicesDto;
        }
        public async Task<InvoicesDto> GetByInvoiceNo(string invoiceNo,CancellationToken cancellationToken)
        {
            var invoices = await _readDbContext.ReadSet<Invoices>().Where(p => p.InvoiceNo.Equals(invoiceNo))
                           .FirstOrDefaultAsync();


            var invoicesDto = _mapper.Map<InvoicesDto>(invoices);
            return invoicesDto;
        }
        public async Task<List<InvoicesDto>> GetLatestInvoices(CancellationToken cancellationToken)
        {
            var invoices = await _readDbContext.ReadSet<Invoices>()
                .Include(p=>p.Payments)
                .Where(p=>p.status==InvoiceStatus.Pending)
                .OrderByDescending(p=>p.IssueDate)
                .Take(5).ToListAsync(cancellationToken);
            var latestInvoicesDto = _mapper.Map<List<InvoicesDto>>(invoices);
            return latestInvoicesDto;
        }
        public async Task<List<MedicalInvoiceDto>> PatientsMedicalRecordInvoices()
        {
            var patientMedicalInvoices = await _readDbContext.ReadSet<MedicalRecords>()
     .OrderByDescending(p => p.CreatedAt)
     .Take(5)
     .Select(p => new MedicalInvoiceDto
     {
         DoctorName = $"{p.Appointment.Doctor.FirstName} {p.Appointment.Doctor.LastName}",
         PatientName = $"{p.Appointment.Patient.FirstName} {p.Appointment.Patient.LastName}",
         MedicalRecordDate = p.CreatedAt,
         PrescriptionItems = p.Prescriptions
              .SelectMany(pr => pr.PrescriptionItems)
             .Select(pi => new InvoicePrescriptionItemsDto
             {
                 Dosage = pi.Dosage,
                 MedicationName = pi.MedicationName,
                 Frequency = pi.Frequency,
                 Duration = pi.Duration
             }).ToList()
     })
     .ToListAsync();
            return patientMedicalInvoices;
        }
    

       public async Task<List<MedicalInvoiceDto>> PatientMedicalRecordInvoicesByAppointmentNumber(string AppointmentNumber)
        {
            var patientMedicalInvoices = await _readDbContext.ReadSet<MedicalRecords>()

      .Where(p => p.Appointment.AppointmentNumber == AppointmentNumber)  // filter first
     .OrderByDescending(p => p.CreatedAt)
     .Take(5)
     .Select(p => new MedicalInvoiceDto
     {
         DoctorName = $"{p.Appointment.Doctor.FirstName} {p.Appointment.Doctor.LastName}",
         PatientName = $"{p.Appointment.Patient.FirstName} {p.Appointment.Patient.LastName}",
         MedicalRecordDate = p.CreatedAt,
         PrescriptionItems = p.Prescriptions
              .SelectMany(pr => pr.PrescriptionItems)
             .Select(pi => new InvoicePrescriptionItemsDto
             {
                 Dosage = pi.Dosage,
                 MedicationName = pi.MedicationName,
                 Frequency = pi.Frequency,
                 Duration = pi.Duration
             }).ToList()
     })
     .ToListAsync();
            return patientMedicalInvoices;
        }

     
    }
}
