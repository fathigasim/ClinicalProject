using AutoMapper;
using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.DoctorsCommandQueries.Dto;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectApplication.PatientsCommandQueries.Dto;
using ClinicProjectApplication.Payment.Command;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectApplication.PaymentReports.Dto;
using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectApplication.PrescriptionsItems.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Models;
using DefaultAuthenticationApplication.PatientsCommandQueries.Dto;



namespace ClinicProjectApplication
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {



            // Order
            //CreateMap<Invoices, CreatePaymentInvoiceCommand>();
            //CreateMap<CreatePaymentInvoiceCommand, Invoices>();
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest=>dest.DayOfWeek,opt=>opt.MapFrom(str=>str.DayOfWeek.ToString()))
                .ReverseMap();
            CreateMap<Invoices, InvoicesDto>().ReverseMap();
            CreateMap<Payments, PaymentDto>()
                        .ForMember(dest => dest.InvoiceNo, opt => opt.MapFrom(src => src.Invoice.InvoiceNo))
                .ReverseMap();
            CreateMap<Invoices, InvoicesDto>().ReverseMap();
            CreateMap<MedicalRecords, MedicalRecordDto>().ReverseMap();
            CreateMap<Prescriptions, PrescriptionsDto>()
                  .ForMember(dest => dest.PrescriptionItemsDto,
               opt => opt.MapFrom(src => src.PrescriptionItems))
                .ReverseMap();
            CreateMap<PrescriptionItems, PrescriptionItemsDto>().ReverseMap();

            //CreateMap<DoctorSchedule, DoctorScheduleDto>()
            //    .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src =>  $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
            //    .ForMember(dest=>dest.ScheduleDate,opt=>opt.MapFrom(src=> src.ScheduledDate))
            //    .ReverseMap();

            CreateMap<Payments,CreatePaymentCommand>().ReverseMap();
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<Patient, PatientDto>()
               .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Id))
               .ReverseMap();

            CreateMap<Appointment, NotInvoicedAppointmentDto>()
                 
                 .ReverseMap();

            CreateMap<DoctorSchedule, DoctorScheduleDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ForMember(dest => dest.ScheduleDate, opt => opt.MapFrom(src => src.ScheduledDate))
             .ReverseMap();

            CreateMap<Payments, PaymentReportDto>()
                 .ForMember(dest => dest.InvoiceNo, opt => opt.MapFrom(src => src.Invoice.InvoiceNo))
                ;
            CreateMap<MonthlyPaymentSummary, MonthlyTotalPaymentsDto>()
                
               ;
            //CreateMap<OrderItem, OrderItemDto>()
            //    .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name));

        }


    }
}
