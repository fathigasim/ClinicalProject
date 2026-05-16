using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetInvoiceById
{
    public class GetInvoiceByInvoiceNumberQueryHandler : IRequestHandler<GetInvoiceByInvoiceNumberQuery,Result<InvoicesDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        public GetInvoiceByInvoiceNumberQueryHandler(IInvoiceRepository invoiceRepository,
            IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }
        public async Task<Result<InvoicesDto>> Handle(GetInvoiceByInvoiceNumberQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetInvoiceByInvoiceNumberAsync(request.invoiceNo, cancellationToken);
            if (invoice == null) {
                return Result<InvoicesDto>.Failure("No invoice with this number found");
            }

            var invoiceDto = _mapper.Map<InvoicesDto>(invoice);
            return Result<InvoicesDto>.Success(invoiceDto);
        }
    }
}
