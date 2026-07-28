using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Settings
{
    public class StripeSettings
    {

        public const string StripeSetting= "StripeSettings";
         public string SecretKey { get; set; }
        public string WebhookSecret { get; set; }
    
    }
}
