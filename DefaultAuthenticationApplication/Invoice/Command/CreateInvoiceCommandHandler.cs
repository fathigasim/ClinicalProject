using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Invoice.Command
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<string>>
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
        public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var appointment=await _appointmentRepository.GetByAppointmentNumberAsync(request.AppointmentNo, cancellationToken);
            if (appointment == null)
            {
                return Result<string>.Failure("Appointment not found for this patient");
            }

            var sequence =await _sequenceService.GenerateInvoiceNumberAsync();

            var invoice = new Invoices
            {
                InvoiceNo = sequence,
                AppointmentId = appointment.Id,
                TotalAmount=request.TotalAmount,
            }; //_mapper.Map<Invoices>(request);
             
            
         
            await  _invoiceRepository.AddAsync(invoice);
            return Result<string>.Success($"Invoice Number: {invoice.InvoiceNo} for patient {appointment.Patient.FirstName} {appointment.Patient.LastName}");
        }
    }
}
