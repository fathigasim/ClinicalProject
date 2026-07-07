using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Dtos
{
    public class InvoicesDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus status { get; set; } = InvoiceStatus.Pending;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public PaymentDto Payment { get; set; }
    }
}
