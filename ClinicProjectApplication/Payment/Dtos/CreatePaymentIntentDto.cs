using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Dtos
{
    public class CreatePaymentIntentDto
    {
        public string clientSecret  { get; set; }
    }
}
