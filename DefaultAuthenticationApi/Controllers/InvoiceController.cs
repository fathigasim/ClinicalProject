using ClinicProjectApplication.Invoice.Command;
using ClinicProjectApplication.Invoice.Queries;
using ClinicProjectApplication.Invoice.Queries.GetAllInvoices;
using ClinicProjectApplication.Invoice.Queries.GetInvoiceById;
using ClinicProjectApplication.Invoice.Queries.GetLatestInvoices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClinicProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<InvoiceController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllInvoicesQuery();
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{invoiceNo}")]
        public async Task<IActionResult> Get(string invoiceNo)
        {
            var query = new GetInvoiceByInvoiceNumberQuery(invoiceNo);
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("LatestInvoices")]
        public async Task<IActionResult> GetLatestInvoices()
        {
            var query = new GetLatestInvoicesQuery();
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // POST api/<InvoiceController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateInvoiceCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT api/<InvoiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InvoiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
