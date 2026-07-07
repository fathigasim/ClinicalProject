using ClinicProjectApplication.Prescription;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PrescriptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<PrescriptionsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
             
            return Ok();
        }

        // GET api/<PresdcriptionsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PrescriptionsController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreatePrescriptionCommand command)
        {
            var result=  await _mediator.Send(command);
          return  result.IsSuccess ? Ok(result) :BadRequest(result.ErrorMessage);
            
        }

        // PUT api/<PrescriptionsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PrescriptionsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
