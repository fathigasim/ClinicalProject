using ClinicProjectApplication.Exceptions;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Events;
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
        private readonly IMessagePublisher _messagePublisher;
        public WebhookCommandHandler(IStripeService stripeService,
            IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository,
            IPublisher publisher,
            ILogger<WebhookCommandHandler> logger,
            IMessagePublisher messagePublisher)
        {
            _stripeService = stripeService;
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _publisher = publisher;
            _logger = logger;   
            _messagePublisher = messagePublisher;
        }
        public async Task<Unit> Handle(WebhookCommand request, CancellationToken cancellationToken)
        {
           
                var paymentResult = await _stripeService.WebHook(request.RawBody, request.Signature);

                if (paymentResult != null)
                {
                var payment = Payments.Create(Guid.Parse(paymentResult.invoiceId),
                    paymentResult.amount, paymentResult.currency, paymentResult.patientEmail, paymentResult.intentId, paymentResult.status);
                 
                    await _paymentRepository.AddAsync(payment, cancellationToken);                    
               await _publisher.Publish(new InvoicePaidNotification(Guid.Parse(paymentResult.invoiceId), paymentResult.patientEmail));
               
              await _messagePublisher.PublishAsync(new PaymentCreatedEvent(Guid.Parse(paymentResult.invoiceId), paymentResult.patientEmail), "invoice.paid", cancellationToken);
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
