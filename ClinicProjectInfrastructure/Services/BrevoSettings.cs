using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Services
{
    public class BrevoSettings
    {
        public string ApiKey { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public string SenderName { get; set; } = default!;
        public string BaseUrl { get; set; } = "https://api.brevo.com/v3/";
    }
}
