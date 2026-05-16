using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Invoice.Notifications;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Payment.Command
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand,Result<string>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;
        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository,
            IMapper mapper,
          IPublisher publisher)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
          _publisher= publisher;
        }
        public  async Task<Result<string>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {

            var invoice =await _invoiceRepository.GetInvoiceByInvoiceNumberAsync(request.InvoiceNo,cancellationToken);
            if (invoice == null)
            {
                return Result<string>.Failure($"Invoice No {request.InvoiceNo} not found");
            }
           var payments=   _mapper.Map<Payments>(request);
            if (payments == null) {
                return Result<string>.Failure("Sorry Payment did not complete successfully");
            }
            if (payments.CashLimitExceeded())
            {

                return Result<string>.Failure("Sorry cash payment exceeded pay with card");
            }
            payments.InvoiceId = invoice.Id;
           await _paymentRepository.AddAsync(payments);
            await _publisher.Publish(new InvoicePaidNotification(payments.InvoiceId));
            return Result<string>.Success("Payment recieved successfully");
        }
    }
}
