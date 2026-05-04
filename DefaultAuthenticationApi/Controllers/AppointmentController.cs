using ClinicProjectApplication.Appointments.AppointmentCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AppointmentController(IMediator mediator)
        {
            _mediator  =mediator;
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateAppointmentCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAppointmentCommand updatecommand)
        {
            var result = await _mediator.Send(updatecommand);

            return Ok(result);
        }

        [HttpPost("Cancel-Appointment")]
        public async Task<IActionResult> CancelAsync([FromBody] CancelAppointmentCommand cancelAppointment)
        {
            var result = await _mediator.Send( cancelAppointment);

            return Ok(result);
        }
    }
}
