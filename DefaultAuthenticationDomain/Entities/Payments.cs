using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Payments
    {
        public Guid Id  { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoices Invoice { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public DateTime PaidAt { get; set; }
        
    }
}
