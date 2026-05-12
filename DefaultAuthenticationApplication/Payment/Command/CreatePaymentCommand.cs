using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public record CreatePaymentCommand :IRequest<Result<string>>,ITransactionalRequest
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentMethod { get; set; } = PaymentType.Cash;
    }
}
