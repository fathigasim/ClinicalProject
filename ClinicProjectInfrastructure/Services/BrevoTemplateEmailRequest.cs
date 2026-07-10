using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Services
{
    public class BrevoTemplateEmailRequest
    {
        public string To { get; set; } = default!;
        public long TemplateId { get; set; }
        public Dictionary<string, object> Params { get; set; } = new();
    }
}
