using ClinicProjectApi.GeneratePdf;
using ClinicProjectApi.ReportServices;
using ClinicProjectApplication.Interfaces;
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
        public async Task< IActionResult> GetDailySalesSum(CancellationToken cancellationToken)
        {
           var result= await mediator.Send(new DailyPaymentSalesQuery(),cancellationToken);
            return Ok(result.Data);
        }

        [HttpGet("GetPaymentsByDate")]
        public async Task<IActionResult> GetDailySalesSum([FromQuery]DateTime date, [FromQuery]int page,[FromQuery]int pageSize,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPaymentsByDateReportQuery(date,page,pageSize), cancellationToken);
            return Ok(result);
        }

        


        [HttpGet("GetMonthlyTotals")]
        public async Task<IActionResult> GetMonthlyTotals(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetMonthlyPaymentsTotalsQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetPaymentsByDateReportPdf")]
        public async Task<IActionResult> GetPaymentsByDateReportPdf([FromQuery] DateTime date, CancellationToken cancellationToken)
        {
         //   var payments =   await _paymentService.GetPaymentsByDateAsync(date); // your existing query logic
            var payments = await   mediator.Send(new GetPaymentsByDatePdfReportQuery(date), cancellationToken);
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


        [HttpGet("GetPaymentsByDateReport")]
        public async Task<IActionResult> GetPaymentsByDateReport(
    [FromQuery] DateTime date,CancellationToken cancellationToken,
    [FromQuery] string format = "pdf")
        {
            var payments = await mediator.Send(new GetPaymentsByDatePdfReportQuery(date), cancellationToken); // no "Pdf" in the query name anymore — it's format-agnostic

            if (payments == null || !payments.Any())
                return NotFound(new { isSuccess = false, errorMessage = "No payments made on this day" });

            var reportData = new PaymentReportData
            {
                ReportDate = date,
                Payments = payments.Select(p => new PaymentReportRow
                {
                    CustomerId = p.CustomerId,
                    InvoiceNo = p.InvoiceNo,
                    Amount = p.Amount,
                    PaidAt = p.PaidAt,
                    PaymentMethod = p.PaymentMethod.ToString(),
                }).ToList(),
                Total = payments.Sum(p => p.Amount)
            };

            IReportExporter<PaymentReportData> exporter = format.ToLowerInvariant() switch
            {
                "xlsx" or "excel" => new PaymentReportExcelExporter(),
                "csv" => new PaymentReportCsvExporter(),
                "pdf" => new PaymentReportPdfExporter(),
                _ => throw new ArgumentException($"Unsupported format: {format}")
            };

            var bytes = exporter.Export(reportData);
            var fileName = $"payment-report-{date:yyyy-MM-dd}.{exporter.FileExtension}";
            return File(bytes, exporter.ContentType, fileName);
        }
    }
}
