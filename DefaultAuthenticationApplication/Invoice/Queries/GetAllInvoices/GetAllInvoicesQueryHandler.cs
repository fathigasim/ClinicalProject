using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Invoice.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetAllInvoices
{
    public class GetAllInvoicesQueryHandler : IRequestHandler<GetAllInvoicesQuery, Result<List<InvoicesDto>>>
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        public GetAllInvoicesQueryHandler(IInvoiceService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        public async Task<Result<List<InvoicesDto>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoiceService.GetAllInvoices(cancellationToken);
            if (invoices == null || !invoices.Any())
            {
                return Result<List<InvoicesDto>>.Failure("No invoices found.");
            }
            var invoicesDto = _mapper.Map<List<InvoicesDto>>(invoices);
            return Result<List<InvoicesDto>>.Success(invoicesDto);
        }
    }
}