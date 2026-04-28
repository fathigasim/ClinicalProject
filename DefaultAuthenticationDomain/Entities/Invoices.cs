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
        public InvoiceStatus status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Payments Payments { get; set; }

    }
}
