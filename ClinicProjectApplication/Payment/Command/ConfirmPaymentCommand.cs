using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public record ConfirmPaymentCommand :IRequest<Result<string>>,ITransactionalRequest
    {
        public string PaymentIntentId { get; set; }
        
    }
}
