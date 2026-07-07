using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Dtos
{
    public class PaymentDto
    {
        public Guid InvoiceId { get; set; }
        
        public decimal Amount { get; set; }
        public PaymentType PaymentMethod { get; set; } = PaymentType.Cash;
    }
}
