using ClinicProjectApplication.MedicalRecord.Command;
using ClinicProjectApplication.MedicalRecord.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DefaultAuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicalRecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<MedicalRecordsController>
        [HttpGet("PatientsInvioces")]
        public async Task< IActionResult> Get()
        {
            var invoices = await _mediator.Send( new GetMedicalRecordInvoiceQuery());
            return Ok(invoices);
        }

        [HttpGet("PatientsInviocesByAppointmentNumber")]
        public async Task<IActionResult> PatientsInviocesByAppiontment(string appointmentNo)
        {
            var invoices = await _mediator.Send(new GetMedicalRecordInvoiceByAppiontmentQuery(appointmentNo));
            return Ok(invoices);
        }

        // GET api/<MedicalRecordsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MedicalRecordsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMedicalRecordCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);  
        }

        // PUT api/<MedicalRecordsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MedicalRecordsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
