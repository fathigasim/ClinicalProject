using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Dtos
{
    public record DailyPaymentsDto(string month,string dayofmonth ,decimal amount);
  
}
