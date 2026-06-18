using ClinicProjectApplication.Payment.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IStripeService
    {

        Task<string> PaymentIntent(string InvoiceId, decimal TotalAmount,CancellationToken cancellationToken);
        Task<PaymentIntentDto?> ConfirmPayment(string PaymentIntentId);

        Task<PaymentIntentDto?> WebHook(string json, string signature);
    }
}
