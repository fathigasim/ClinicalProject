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
        public async Task<InvoicesDto> GetPatientInfoByInvoiceId(Guid invoiceId, CancellationToken cancellationToken)
        {
            var invoice = await _readDbContext.ReadSet<Invoices>()
                .Include(p=>p.Appointment).ThenInclude(p=>p.Patient)
                .Where(p => p.Id.Equals(invoiceId))
                
                           .FirstOrDefaultAsync(cancellationToken);
         return    _mapper.Map<InvoicesDto>(invoice);
        }
        public async Task<InvoicesDto> GetByInvoiceNo(string invoiceNo, CancellationToken cancellationToken)
        {
            var invoices = await _readDbContext.ReadSet<Invoices>().Where(p => p.InvoiceNo.Equals(invoiceNo))
                           .FirstOrDefaultAsync();


            var invoicesDto = _mapper.Map<InvoicesDto>(invoices);
            return invoicesDto;
        }
        public async Task<List<InvoicesDto>> GetLatestInvoices(CancellationToken cancellationToken)
        {
            var invoices = await _readDbContext.ReadSet<Invoices>()
                .Include(p => p.Payment)
                .Where(p => p.status == InvoiceStatus.Pending)
                .OrderByDescending(p => p.IssueDate)
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
         PrescriptionItems = p.Prescription
              .PrescriptionItems//.SelectMany(pr => pr.)
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
         PrescriptionItems = p.Prescription.PrescriptionItems
             // .Select(pr => pr.)
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

        public async Task<List<MonthlyInvoiceDto>> GetMonthlyInvoices(CancellationToken cancellationToken)
        {
            var monthlyInvoice =
                await _readDbContext.ReadSet<Invoices>()
                .GroupBy(p => p.CreatedAt.Month.ToString())
                .Select(g => new MonthlyInvoiceDto { InvoiceMonth = g.Key, InvoiceMonthTotal = g.Sum(p => p.TotalAmount) }).ToListAsync(cancellationToken);
            return monthlyInvoice;

            // .ToListAsync(cancellationToken);
        }

        public async Task<List<WeeklyInvoiceDto>> GetWeeklyInvoices(CancellationToken cancellationToken)
        {
            var invoices =
                await _readDbContext.ReadSet<Invoices>()
                .ToListAsync();
            var weeklyInvoice = invoices.GroupBy(p => p.CreatedAt.DayOfWeek.ToString()).Select(g => new WeeklyInvoiceDto
            { WeeklyInvoice = g.Key, WeeklyInvoiceTotal = g.Sum(p => p.TotalAmount) }).ToList();
             
            return weeklyInvoice;
        }

        public async Task<List<DailyInvoiceDto>> GetDailyInvoices(CancellationToken cancellationToken)
        {
            var dailyInvoice =
                await _readDbContext.ReadSet<Invoices>()
                .Where(p=>p.CreatedAt.Month == DateTime.Now.Month)
                .GroupBy(p =>new { p.CreatedAt.Day,p.CreatedAt.Month })
                .Select(g => new DailyInvoiceDto { DailyInvoiceDate = g.Key.Day.ToString("D2")+'-'+ g.Key.Month.ToString("D2"), DayOfMonth=g.Key.Month.ToString("D2"), DailyInvoiceDateTotal = g.Sum(p => p.TotalAmount) }).ToListAsync(cancellationToken);
            return dailyInvoice;
        }
    }
}