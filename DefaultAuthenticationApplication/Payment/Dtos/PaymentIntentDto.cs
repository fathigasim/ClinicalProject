using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Dtos
{
    public class PaymentIntentDto
    {
        public string intentId { get; set; }

        public string status { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string customerId { get; set; }
        public string invoiceId { get; set; }
        public string patientEmail { get; set; }
        

    }
}
