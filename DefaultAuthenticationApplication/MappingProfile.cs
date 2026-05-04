using AutoMapper;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Invoice;
using ClinicProjectApplication.Prescription.Dtos;
using ClinicProjectApplication.PrescriptionsItems.Dtos;
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {



            // Order
            CreateMap<Invoices, CreateInvoiceCommand>();
            CreateMap<CreateInvoiceCommand, Invoices>();

            CreateMap<Prescriptions, PrescriptionsDto>().ReverseMap();
            CreateMap<PrescriptionItems, PrescriptionItemsDto>().ReverseMap();

            CreateMap<WeeklySchedule, WeeklyScheduleDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => $"{src.Doctor.FirstName} {src.Doctor.LastName}"))
                .ReverseMap();
            
            //CreateMap<OrderItem, OrderItemDto>()
            //    .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name));

        }


    }
}
