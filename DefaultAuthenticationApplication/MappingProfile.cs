using AutoMapper;
using ClinicProjectApplication.Invoice;
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

            //CreateMap<OrderItem, OrderItemDto>()
            //    .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.Name));

        }


    }
}
