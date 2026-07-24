
using ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorCommand;
using ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule;
using ClinicProjectApplication.DoctorsCommandQueries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DefaultAuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 //   [Authorize(Policy = "AdminOnly")]
    public class DoctorsController : ControllerBase
    {
        private readonly ILogger<DoctorsController> _logger;
        private readonly IMediator _mediator;
        public DoctorsController(ILogger<DoctorsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("Scheduled-Doctors")]
        public async Task<IActionResult> GetScheduledDoctors(CancellationToken ct)
        {
          var listedDoctors=  await _mediator.Send(new GetListedDoctorQuery());
            return Ok(listedDoctors);
        }
        [HttpGet("Doctors-List")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDoctors(int page, int pageSize,CancellationToken ct)
        {
            var doctors = await _mediator.Send(new GetAllDoctorQuery(page,pageSize));

            return Ok(doctors);

        }

        [HttpGet("Doctors-Shift")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTodaysDoctorsShifts()
        {
            var doctors = await _mediator.Send(new GetTodaysDoctorShiftQuery());

            return Ok(doctors);

        }

        [HttpGet("Doctors-Shift-By-Date")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsShiftsByDate()
        {
            var doctors = await _mediator.Send(new GetTodaysDoctorShiftQuery());

            return Ok(doctors);

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
        [AllowAnonymous]
        [HttpGet("available-slots")]

        public async Task<IActionResult> DoctorAvailableSlots( Guid doctorId, DateOnly appointmentDate)
        {
            var availableSlots = await _mediator.Send(new GetDoctorsAvailableSlotsQuery { DoctorId = doctorId, AppointmentDate = appointmentDate });

            return Ok(availableSlots);

        }
        [AllowAnonymous]
        [HttpGet("available-slots-by-date")]
        public async Task<IActionResult> DoctorAvailableSlotsByDate(Guid doctorId, DateOnly date)
        {
            var availableSlots = await _mediator.Send(new GetDoctorsAvailableSlotsByDateQuery ( doctorId, date ));

            return Ok(availableSlots);

        }
        [HttpPost("schedule")]

        public async Task<IActionResult> CreateDoctorSchedule([FromBody] CreateDoctorScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess) { 
            return Ok(result.Data);
            }
            return UnprocessableEntity(new { message = result.ErrorMessage });
        }

        [HttpGet("doctorsSchedule")]

        public async Task<IActionResult> CreateDoctorSchedule( )
        {
            var result = await _mediator.Send(new GetWeeklyScheduleQuery());
           
                return Ok(result);
           
      
        }
        [HttpPut("updatedoctorsSchedule/{id:guid}")]

        public async Task<IActionResult> CreateDoctorSchedule(Guid id,UpdateDoctorScheduleCommand cmd)
        {
            var result = await _mediator.Send(cmd);

            return Ok(result);


        }

        [HttpGet("doctorScheduleById/{id:Guid}")]

        public async Task<IActionResult> GetDoctorSchedule(Guid id)
        {
            var result = await _mediator.Send(new GetDoctorScheduleByIdQuery(id));

            return Ok(result.Data);


        }

    }
}
