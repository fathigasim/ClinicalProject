using ClinicProjectApplication.Exceptions;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Payment.Command;
using ClinicProjectApplication.Payment.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Ocsp;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ClinicProjectInfrastructure.Services
{
    public class StripeService : IStripeService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly IConfiguration _configuration;
        public StripeService(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
           _configuration = configuration;
            _httpContext = httpContext;
         
        }
        public async Task<string> PaymentIntent(string InvoiceId, decimal TotalAmount) {

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(TotalAmount * 100), //  always in cents
                Currency = "sar",
                Metadata = new Dictionary<string, string>
        {
            { "invoiceId", InvoiceId } //  attach your business data here
        }
            };
            var client = new StripeClient(_configuration["Stripe:SecretKey"]);
         var    _paymentIntentService = new PaymentIntentService(client);

            var intent = await _paymentIntentService.CreateAsync(options);

            return intent.ClientSecret;
        }

        public async Task<PaymentIntentDto?> ConfirmPayment(string PaymentIntentId)
        {

            var service = new PaymentIntentService();
            var intent = await service.GetAsync(PaymentIntentId);
        
            //intent.Metadata.TryGetValue("InvoiceId", out var invoiceId)
            //var invoiceId = intent.Metadata["InvoiceId"];
            if (intent.Status != "succeeded")
            { // always verify on backend, never trust frontend
                return new PaymentIntentDto()
                {
                    customerId = intent.CustomerId,
                    amount = intent.Amount,
                    currency = intent.Currency,
                    intentId = intent.Id,
                    status = intent.Status,
                    InvoiceId = intent.Metadata["InvoiceId"]
                };
            }
            return null;
        }

        public async Task<PaymentIntentDto?> WebHook(string json, string signature)
        {

            try
            {
               // var json = await new StreamReader(_httpContext.HttpContext.Request.Body).ReadToEndAsync();

                var webhookSecret = _configuration["Stripe:WebhookSecret"];

                if (string.IsNullOrEmpty(webhookSecret))
                {
                    throw new InvalidOperationException("Stripe webhook secret is not configured.");
                }

                // ✅ Always verify the webhook signature
                //var stripeEvent = EventUtility.ConstructEvent(json,
                //   _httpContext.HttpContext.Request.Headers["Stripe-Signature"]
                //    , _configuration["Stripe:WebhookSecret"]
                //    , throwOnApiVersionMismatch: false
                //    );

                var stripeEvent = EventUtility.ConstructEvent(
       json,
       signature,
       webhookSecret,
       throwOnApiVersionMismatch: false
   );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var intent = stripeEvent.Data.Object as PaymentIntent;
                  
                        return new PaymentIntentDto()
                        {
                            customerId = intent.CustomerId,
                            amount = intent.Amount/100m,
                            currency = intent.Currency,
                            intentId = intent.Id,
                            status = intent.Status,
                            InvoiceId = intent.Metadata["invoiceId"]
                        };
                  
                    //         await _paymentService.SavePayment(intent); // safe to save now
                }


             
            }
            catch (StripeException ex)
            {
                //_logger.LogWarning("web secert error message {error message}", ex.Message);
                throw new InvalidWebhookSignatureException(ex.Message);
            }
            return null;
        }
    }
    }

