using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Payments :BaseEntity, IAuditableEntity
    {
        private  Payments(Guid invoiceId, decimal amount, string currency, string? customerId, string paymentId,string status)
        {
           InvoiceId = invoiceId;
            Amount = amount;
            Currency = currency;
            PaymentId = paymentId;
            Status = status;
            CustomerId = customerId;
        }
        public Guid InvoiceId { get;private set; }

        private Invoices _Invoice;
        public Invoices Invoice =>_Invoice;
        public decimal Amount { get;private set; }
        public string? CustomerId { get;private set; }

        public string Currency  { get; private set; }
        public string PaymentId { get;private set; }
        public string Status { get;private set; }
    
        public PaymentType PaymentMethod { get;private set; } = PaymentType.Cash;
        public DateTime PaidAt { get;private set; }= DateTime.Now;

        public static Payments Create(Guid invoiceId, decimal amount,string currency, string? customerId ,string paymentId, string status)
        {
            if (invoiceId.Equals(Guid.Empty)) { 
               throw new KeyNotFoundException("Invoice is required");
            }
          return  new Payments( invoiceId,  amount,currency ,customerId, paymentId,  status);
        }

        public void UpdateInvocie(Guid invoice)
        {
            InvoiceId=invoice;
        }
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
