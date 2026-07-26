using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PaymentReports.Dto
{
    public class PaymentReportDto
    {
      

        public string InvoiceNo { get; set; }
  
        public decimal Amount { get;  set; }
        public string? CustomerId { get;  set; }

        public string Currency { get;  set; }
        public string PaymentId { get;  set; }
        public string Status { get;  set; }
    }
}
