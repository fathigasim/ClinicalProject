using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public class WebhookCommandHandler : IRequestHandler<WebhookCommand,Unit>
    {
        private readonly IStripeService _stripeService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<WebhookCommandHandler> _logger;
        public WebhookCommandHandler(IStripeService stripeService,
            IPaymentRepository paymentRepository,

            ILogger<WebhookCommandHandler> logger)
        {
            _stripeService = stripeService;
            _paymentRepository = paymentRepository;
            _logger = logger;   
        }
        public async Task<Unit> Handle(WebhookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var paymentResult = await _stripeService.WebHook(request.RawBody,request.Signature);
                if (paymentResult != null)
                {
                    var payment = new Payments()
                    {
                        Amount = paymentResult.amount,
                        Status = paymentResult.status,
                        //CustomerId = paymentResult.customerId,
                        InvoiceId = Guid.Parse(paymentResult.InvoiceId),
                        PaymentId = paymentResult.intentId,
                        Currency = paymentResult.currency,
                    };
                    await _paymentRepository.AddAsync(payment,cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("webhook handler error {error}", ex.Message);
                throw new Exception(ex.Message);
            }

            return Unit.Value;
        }
    }
}
