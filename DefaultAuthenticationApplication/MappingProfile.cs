using AutoMapper;
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
             CreateMap<Invoices, InvoicesDto>().ReverseMap();
            CreateMap<Payments, PaymentDto>().ReverseMap();
            CreateMap<Prescriptions, PrescriptionsDto>().ReverseMap();
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

            CreateMap<MedicalRecords, MedicalRecordDto>().ReverseMap();
          
            CreateMap<PrescriptionItems, PrescriptionItemDto>();
            //CreateMap<OrderItem, OrderItemDto>()
            //    .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name));

        }


    }
}
