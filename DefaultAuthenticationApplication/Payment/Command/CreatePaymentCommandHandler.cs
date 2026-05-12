using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Payment.Command
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand,Result<string>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IMapper mapper) {
           _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
      public  async Task<Result<string>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {


           var payments=   _mapper.Map<Payments>(request);
            if (payments == null) {
                return Result<string>.Failure("Sorry Payment did not complete successfully");
            }
            if (payments.CashLimitExceeded())
            {

                return Result<string>.Failure("Sorry cash payment exceeded pay with card");
            }

           await _paymentRepository.AddAsync(payments);
            return Result<string>.Success("Payment recieved successfully");
        }
    }
}
