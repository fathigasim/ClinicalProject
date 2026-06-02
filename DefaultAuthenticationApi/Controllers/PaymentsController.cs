using ClinicProjectApplication.Payment.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ClinicProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ISender _sender; 
        private readonly IConfiguration _configuration;
        public PaymentsController(ISender sender, IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(CreatePaymentCommand command)
        {
         var result=    await _sender.Send(command);
            return Ok(result); 
        }

        // Step 1: Create a PaymentIntent BEFORE the user pays
        // Frontend calls this first to get the clientSecret
        [HttpPost("create-payment-intent")] 
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentRequest req)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(req.TotalAmount * 100), //  always in cents
                Currency = "sar",
                Metadata = new Dictionary<string, string>
        {
            { "invoiceId", req.InvoiceId } //  attach your business data here
        }
            };

            var service = new PaymentIntentService(new StripeClient(_configuration["stripe:SecretKey"]));
            var intent = await service.CreateAsync(options);

            return Ok(new { clientSecret = intent.ClientSecret }); // ✅ send back to frontend
        }

        // Step 2: Confirm and save after Stripe processes payment
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest req)
        {
            var service = new PaymentIntentService();
            var intent = await service.GetAsync(req.PaymentIntentId);

            if (intent.Status != "succeeded") // ✅ always verify on backend, never trust frontend
                return BadRequest("Payment not successful.");

            // ✅ Now save to your DB
        //    await _paymentService.SavePayment(intent);
            return Ok();
        }

        // Stripe calls this even if user closes browser mid-payment
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // ✅ Always verify the webhook signature
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], "your_webhook_secret");

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var intent = stripeEvent.Data.Object as PaymentIntent;
       //         await _paymentService.SavePayment(intent); // safe to save now
            }

            return Ok();
        }

        public record CreatePaymentRequest(string InvoiceId,decimal TotalAmount);
        public record ConfirmPaymentRequest(string PaymentIntentId);
    }
}
