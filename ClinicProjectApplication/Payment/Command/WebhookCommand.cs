using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public class WebhookCommand :IRequest<Unit>,ITransactionalRequest
    {
        public string RawBody { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
