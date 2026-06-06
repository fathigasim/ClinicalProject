using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Result<string>>
    {
        private readonly IStripeService _stripeService;
        private readonly IPaymentRepository _paymentRepository;
        public ConfirmPaymentCommandHandler(IStripeService stripeService, IPaymentRepository paymentRepository)
        {
            _stripeService = stripeService;
            _paymentRepository = paymentRepository;
        }
        public async Task<Result<string>> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {

            var confirmPayment = await _stripeService.ConfirmPayment(request.PaymentIntentId);
            if (confirmPayment != null)
            {
                var payment = new Payments
                {
                    InvoiceId = Guid.Parse(confirmPayment.InvoiceId),
                     Amount=confirmPayment.amount,
                     Status=confirmPayment.status,
                     PaymentId=confirmPayment.intentId,
                     CustomerId=confirmPayment.customerId,
                };
              await  _paymentRepository.AddAsync(payment);
                return Result<string>.Success("Payment Confirmed Successfully");
            }
            return Result<string>.Failure("Payment failed ");

        }
    }
}
