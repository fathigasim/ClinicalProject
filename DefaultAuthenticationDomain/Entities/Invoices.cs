using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class Invoices
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus status { get; set; }= InvoiceStatus.Pending;
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public Payments Payments { get; set; }

        public void MarkAsPaid()
        {
            if (status == InvoiceStatus.Paid)
            {
                throw new InvalidOperationException("Invoice is already marked as paid.");
            }
            status = InvoiceStatus.Paid;
        }
         public void MarkAsCancelled()
        {
            if (status == InvoiceStatus.Cancelled)
            {
                throw new InvalidOperationException("Invoice is already marked as cancelled.");
            }
            status = InvoiceStatus.Cancelled;
        }
        public bool CashLimitExceeded()
        {
            if (Payments.PaymentMethod == PaymentType.Cash && TotalAmount > 1000) // Example cash limit
            {
              return true;  // Cash limit exceeded
            }
            return false;
        }
    }
}
