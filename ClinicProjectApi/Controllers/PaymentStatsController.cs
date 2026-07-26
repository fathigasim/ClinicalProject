using ClinicProjectApplication.Payment.Queries;
using ClinicProjectApplication.PaymentReports.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetDailySalesSum([FromQuery]DateTime date)
        {
            var result = await mediator.Send(new GetPaymentsByDateReportQuery(date));
            return Ok(result.Data);
        }
    }
}
