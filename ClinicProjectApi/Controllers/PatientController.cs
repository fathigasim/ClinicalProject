
using ClinicProjectApplication.PatientsCommandQueries.Command.CreatePatient;
using ClinicProjectApplication.PatientsCommandQueries.Command.UpdatePatient;
using ClinicProjectApplication.PatientsCommandQueries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefaultAuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     [Authorize(Roles = "Admin,User")]
   // [Authorize(Policy = "AdminUser")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PatientController(IMediator mediator) { 
         _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet("TodaysPatients")]
        public async Task<IActionResult> GetTodaysPatients()
        {
            var todaysPatients = await _mediator.Send(new GetTodaysPatientsQuery());
            return Ok(todaysPatients);
             
        }

        [HttpGet]
        public async Task<IActionResult> GetPatient([FromQuery]string? q, [FromQuery] int page, [FromQuery] int pageSize)
        {
           var patient= await _mediator.Send(new GetPatientByNameQuery(q,page,pageSize));
            return Ok(patient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _mediator.Send(new GetPatientByIdQuery(id));
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> PostPatient( CreatePatientCommand cmd , CancellationToken ct)
        {
            var  result= await _mediator.Send(cmd, ct);

            return Ok(result);
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientCommand cmd, CancellationToken ct)
        {
            //if (id != cmd.Id)
                cmd.Id = id;
           // return BadRequest("Route id and body id do not match.");

            var result = await _mediator.Send(cmd, ct);
            return Ok(result);
        }
    }
}
