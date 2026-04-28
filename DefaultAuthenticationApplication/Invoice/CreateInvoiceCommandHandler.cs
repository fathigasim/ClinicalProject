using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Invoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<string>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
            private readonly ISequenceService _sequenceService;
        private readonly IMapper _mapper;
        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository,
            ISequenceService sequenceService,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _sequenceService = sequenceService;
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var sequence =await _sequenceService.GenerateInvoiceNumberAsync();

            var invoice = _mapper.Map<Invoices>(request);
            var isCashExceeded= invoice.CashLimitExceeded();//avoid wasting sequence
            if (isCashExceeded) { 
               return Result<string>.Failure("Total Amount exceeds the cash limit.");
            }
            invoice.InvoiceNo = sequence;
         
            await  _invoiceRepository.AddAsync(invoice);
            return Result<string>.Success($"Invoice Created with Number: {invoice.InvoiceNo}");
        }
    }
}
