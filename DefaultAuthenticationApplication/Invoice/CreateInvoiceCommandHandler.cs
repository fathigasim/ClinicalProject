using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Invoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<Unit>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public Task<Result<Unit>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoices()
            {


            };
            throw new NotImplementedException();
        }
    }
}
