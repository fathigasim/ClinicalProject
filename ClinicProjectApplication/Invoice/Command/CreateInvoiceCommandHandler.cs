using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Invoice.Command
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoicesDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IAppointmentRepository _appointmentRepository;
            private readonly ISequenceService _sequenceService;

        private readonly IMapper _mapper;
        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository,
            IAppointmentRepository appointmentRepository,
            ISequenceService sequenceService,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _appointmentRepository = appointmentRepository;
            _sequenceService = sequenceService;
            _mapper = mapper;
        }
        public async Task<Result<InvoicesDto>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var appointment=await _appointmentRepository.GetByAppointmentNumberAsync(request.AppointmentNo, cancellationToken);
            if (appointment == null)
            {
                return Result<InvoicesDto>.Failure("Appointment not found for this patient");
            }

            var sequence =await _sequenceService.GenerateInvoiceNumberAsync();

            var invoice = new Invoices
            {
                InvoiceNo = sequence,
                AppointmentId = appointment.Id,
                TotalAmount=request.TotalAmount,
            };
           var invoiceDto=   _mapper.Map<InvoicesDto>(invoice);
            
         
            await  _invoiceRepository.AddAsync(invoice);
            return Result<InvoicesDto>.Success(invoiceDto);
        }
    }
}
