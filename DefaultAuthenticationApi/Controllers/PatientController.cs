
using ClinicProjectApplication.Patients.Command;
using ClinicProjectApplication.Patients.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefaultAuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminUser")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PatientController(IMediator mediator) { 
         _mediator = mediator;
        }
        [HttpGet("TodaysPatients")]
        public async Task<IActionResult> GetTodaysPatients()
        {
            var todaysPatients = await _mediator.Send(new GetTodaysPatientsQuery());
            return Ok(todaysPatients);
             
        }

        [HttpGet("{phone}")]
        public async Task<IActionResult> GetPatient(string phone)
        {
           var patient= await _mediator.Send(new GetPatientByPhoneQuery(phone));
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> PostPatient( CreatePatientCommand cmd , CancellationToken ct)
        {
            var  result= await _mediator.Send(cmd, ct);

            return Ok(result);
           
        }
    }
}
