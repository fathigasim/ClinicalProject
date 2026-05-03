using ClinicProjectApplication.Doctors.Command.DoctorCommand;
using ClinicProjectApplication.Doctors.Command.DoctorWeeklySchedule;
using ClinicProjectApplication.Doctors.Queries;
using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace DefaultAuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;
        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{email}")]

        public async Task<IActionResult> GetDoctor(string email)
        {
            var doctorId = await _mediator.Send(new GetDoctorByEmailQuery(email));

            return Ok(doctorId);

        }

        [HttpPost]

        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
        {
            var doctorId = await _mediator.Send(command);

            return Ok(doctorId);
            
        }
        [HttpGet("available-slots")]

        public async Task<IActionResult> DoctorAvailableSlots(Guid doctorId, DayOfWeek dayOfWeek)
        {
            var availableSlots = await _mediator.Send(new GetDoctorsAvailableSlotsQuery { DoctorId = doctorId, DayOfWeek = dayOfWeek });

            return Ok(availableSlots);

        }
        [HttpPost("schedule")]

        public async Task<IActionResult> CreateDoctorSchedule([FromBody] CreateWeeklyScheduleCommand command)
        {
            var doctorId = await _mediator.Send(command);

            return Ok(doctorId);

        }
    }
}
