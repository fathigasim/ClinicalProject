using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Dtos
{
    public class MonthlyInvoiceDto
    {
        public string InvoiceMonth { get; set; }

        public decimal InvoiceMonthTotal{ get; set; }
    }
}
