using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Dtos
{
    public class DailyInvoiceDto
    {
        
        public string DailyInvoiceDate { get; set; }
        public string DayOfMonth { get; set; }
        public decimal DailyInvoiceDateTotal { get; set; }
    }
}
