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
        public PaymentType PaymentMethod { get; set; } = PaymentType.Cash;
        public DateTime PaidAt { get; set; }= DateTime.Now;

        public bool CashLimitExceeded()
        {
            if (PaymentMethod == PaymentType.Cash && Amount > 1000) // Example cash limit
            {
                return true;  // Cash limit exceeded
            }
            return false;
        }
    }
}
