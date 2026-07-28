using ClinicProjectApi.GeneratePdf;
using ClinicProjectApplication.Payment.Queries;
using ClinicProjectApplication.PaymentReports.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace ClinicProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentStatsController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        public async Task< IActionResult> GetDailySalesSum()
        {
           var result= await mediator.Send(new DailyPaymentSalesQuery());
            return Ok(result.Data);
        }

        [HttpGet("GetPaymentsByDate")]
        public async Task<IActionResult> GetDailySalesSum([FromQuery]DateTime date, [FromQuery]int page,[FromQuery]int pageSize)
        {
            var result = await mediator.Send(new GetPaymentsByDateReportQuery(date,page,pageSize));
            return Ok(result);
        }

        [HttpGet("GetPaymentsByDateReportPdf")]
        public async Task<IActionResult> GetPaymentsByDateReport([FromQuery] DateTime date)
        {
         //   var payments =   await _paymentService.GetPaymentsByDateAsync(date); // your existing query logic
            var payments = await   mediator.Send(new GetPaymentsByDatePdfReportQuery(date));
            if (payments == null || !payments.Any())
                return NotFound(new { isSuccess = false, errorMessage = "No payments made on this day" });

            var reportData = new PaymentReportData
            {
                ReportDate = date,
                Payments = payments.Select(p => new PaymentReportRow
                {
                    CustomerId = p.CustomerId,
                    InvoiceNo= p.InvoiceNo,
                    Amount = p.Amount,
                    PaidAt = p.PaidAt,
                    PaymentMethod = p.PaymentMethod.ToString(),
                }).ToList(),
                Total = payments.Sum(p => p.Amount)
            };

            var document = new PaymentReportDocument(reportData);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", $"payment-report-{date:yyyy-MM-dd}.pdf");
        }
    }
}
