using ClinicProjectApplication.Payment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public record CreatePaymentIntentCommand :IRequest<string>
    {

        public string InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
       
    }
}
