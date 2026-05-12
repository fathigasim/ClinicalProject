using ClinicProjectApplication.Payment.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ISender _sender; 
        public PaymentsController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(CreatePaymentCommand command)
        {
         var result=    await _sender.Send(command);
            return Ok(result); 
        }
    }
}
