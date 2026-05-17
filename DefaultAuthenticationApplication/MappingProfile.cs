using AutoMapper;
using ClinicProjectApplication.Appointments.Dtos;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Invoice;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectApplication.Patients.Dto;
using ClinicProjectApplication.Payment.Command;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectApplication.PrescriptionsItems.Dtos;
using ClinicProjectDomain.Entities;



namespace ClinicProjectApplication
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {



            // Order
            //CreateMap<Invoices, CreatePaymentInvoiceCommand>();
            //CreateMap<CreatePaymentInvoiceCommand, Invoices>();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Invoices, InvoicesDto>().ReverseMap();
            CreateMap<Payments, PaymentDto>().ReverseMap();
            CreateMap<Invoices, InvoicesDto>().ReverseMap();
            CreateMap<MedicalRecords, MedicalRecordDto>().ReverseMap();
            CreateMap<Prescriptions, PrescriptionsDto>()
                  .ForMember(dest => dest.PrescriptionItemsDto,
               opt => opt.MapFrom(src => src.PrescriptionItems))
                .ReverseMap();
            CreateMap<PrescriptionItems, PrescriptionItemsDto>().ReverseMap();

            CreateMap<WeeklySchedule, WeeklyScheduleDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src =>  $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ReverseMap();

            CreateMap<Payments,CreatePaymentCommand>().ReverseMap();
            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<Patient, PatientDto>()
               .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Id))
               .ReverseMap();

       
          
      
            //CreateMap<OrderItem, OrderItemDto>()
            //    .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name));

        }


    }
}
