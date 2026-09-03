using ClinicProjectDomain.Enums;
using ClinicProjectDomain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public  class Invoices :BaseEntity, IAuditableEntity
    {

        private Invoices(string invoiceNo,Guid appointmentId,decimal totalAmount)
        {
            InvoiceNo = invoiceNo;
            AppointmentId = appointmentId;
            TotalAmount = totalAmount;
        }

        private Invoices(InvoiceStatus invoiceStatus)
        {
          status=invoiceStatus;
        }

        private Appointment _Appointment;
        public Appointment Appointment=>_Appointment;

        private Payments _Payment;
        public Payments Payment => _Payment;
        public string InvoiceNo { get;private set; }
        public Guid AppointmentId { get;private set; }
        
        public decimal TotalAmount  { get;private set; }
        public InvoiceStatus status { get;private set; }= InvoiceStatus.Pending;
        public DateTime IssueDate { get;private set; }= DateTime.UtcNow;
     //   public Payments Payments { get; set; }

        public static Invoices Create(string invoiceNo, Guid appointmentId, decimal totalAmount)
        {
            
            return new Invoices( invoiceNo,  appointmentId,  totalAmount);
        }

        public void UpdateStatus(InvoiceStatus newStatus)
        {
            status = newStatus;
        }
        public  void MarkAsPaid()
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
      
    }
}
