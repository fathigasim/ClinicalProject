using ClinicProjectApplication.Payment.Command;
using ClinicProjectApplication.Payment.PaymentReports;
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
        private readonly ILogger<PaymentsController> _logger;
        public PaymentsController(ISender sender, IConfiguration configuration, ILogger<PaymentsController> logger  )
        {
            _sender = sender;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("MonthlyReport")]
        public async Task<IActionResult> GetAsync()
        {
            var query = new MonthlyPaymentReportQuery();
            var result = await _sender.Send(query);
            return Ok(result);
        }

        [HttpGet("DailyReport")]
        public async Task<IActionResult> GetDailyAsync()
        {
            var query = new DailyPaymentReportQuery();
            var result = await _sender.Send(query);
            return Ok(result);
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
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentCommand req)
        {
            //    var options = new PaymentIntentCreateOptions
            //    {
            //        Amount = (long)(req.TotalAmount * 100), //  always in cents
            //        Currency = "sar",
            //        Metadata = new Dictionary<string, string>
            //{
            //    { "invoiceId", req.InvoiceId } //  attach your business data here
            //}
            //    };

            //    var service = new PaymentIntentService(new StripeClient(_configuration["Stripe:SecretKey"]));
            //    var intent = await service.CreateAsync(options);
           var sercret=   await _sender.Send(req);

            return Ok(new { clientSecret = sercret }); // ✅ send back to frontend
        }

        // Step 2: Confirm and save after Stripe processes payment
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentCommand req)
        {
            //var service = new PaymentIntentService();
            //var intent = await service.GetAsync(req.PaymentIntentId);

            //if (intent.Status != "succeeded") // ✅ always verify on backend, never trust frontend
            //    return BadRequest("Payment not successful.");
        var result=    await _sender.Send(req);
             return result.IsSuccess ? Ok(result) : BadRequest(result.ErrorMessage);
         

        }

        // Stripe calls this even if user closes browser mid-payment
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            // Read raw body BEFORE anything else touches it
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"].ToString();
            var command = new WebhookCommand
            {
                RawBody = json,
                Signature = signature
            };
            await _sender.Send(command);
            //try
            //{
            //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            //    var webhookSecret = _configuration["Stripe:WebhookSecret"];

            //    if (string.IsNullOrEmpty(webhookSecret))
            //    {
            //        throw new InvalidOperationException("Stripe webhook secret is not configured.");
            //    }

            //    // ✅ Always verify the webhook signature
            //    var stripeEvent = EventUtility.ConstructEvent(json,
            //        Request.Headers["Stripe-Signature"]
            //        , _configuration["Stripe:WebhookSecret"]
            //        , throwOnApiVersionMismatch:false
            //        );

            //    if (stripeEvent.Type == "payment_intent.succeeded")
            //    {
            //        var intent = stripeEvent.Data.Object as PaymentIntent;
            //        //         await _paymentService.SavePayment(intent); // safe to save now
            //    }


                return Ok();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogWarning("web secert error message {error message}", ex.Message);
            //    return BadRequest() ; 
            //}
        }

        public record CreatePaymentRequest(string InvoiceId,decimal TotalAmount);
        public record ConfirmPaymentRequest(string PaymentIntentId);
    }
}
