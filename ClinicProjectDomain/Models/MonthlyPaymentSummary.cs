using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Models
{
    public record MonthlyPaymentSummary(int Month, decimal TotalAmount);
}
