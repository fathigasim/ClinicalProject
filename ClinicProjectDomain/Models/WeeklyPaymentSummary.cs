using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Models
{
    public record WeeklyPaymentSummary(string dayOfWeek,decimal total);
   
}
