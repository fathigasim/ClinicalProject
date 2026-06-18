using ClinicProjectApplication.Exceptions;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Notifications;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Enums;
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
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPublisher _publisher;
        private readonly ILogger<WebhookCommandHandler> _logger;
        public WebhookCommandHandler(IStripeService stripeService,
            IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository,
            IPublisher publisher,
            ILogger<WebhookCommandHandler> logger)
        {
            _stripeService = stripeService;
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _publisher = publisher;
            _logger = logger;   
        }
        public async Task<Unit> Handle(WebhookCommand request, CancellationToken cancellationToken)
        {
           
                var paymentResult = await _stripeService.WebHook(request.RawBody, request.Signature);

                if (paymentResult != null)
                {
                    var payment = new Payments()
                    {
                        Amount = paymentResult.amount,
                        Status = paymentResult.status,
                        
                        InvoiceId = Guid.Parse(paymentResult.invoiceId),
                        PaymentId = paymentResult.intentId,
                        Currency = paymentResult.currency,
                        //paymentResult.patientEmail
                    };
                    await _paymentRepository.AddAsync(payment, cancellationToken);                    
               await _publisher.Publish(new InvoicePaidNotification(Guid.Parse(paymentResult.invoiceId), paymentResult.patientEmail));
               
              
            }
            
         
            //catch (Exception ex)
            //{
            //    // Business logic error — log and return 200 to prevent Stripe retries
            //    _logger.LogError(ex, "Webhook processing failed: {error}", ex.Message);
            //}

            return Unit.Value;
        }
    }
}
