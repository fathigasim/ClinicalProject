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
    public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, string>
    {
        private readonly IStripeService _stripeService;
        public CreatePaymentIntentCommandHandler(IStripeService stripeService)
        {
            _stripeService = stripeService; 
        }
        public async Task<string> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
          var clientSecret=   await _stripeService.PaymentIntent(request.InvoiceId, request.TotalAmount);
            if (string.IsNullOrEmpty(clientSecret))
            {
                  throw new KeyNotFoundException(nameof(clientSecret));
            }
            return clientSecret;
        }
    }
}
