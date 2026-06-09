using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Invoice.Dtos;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Queries.GetInvoiceByDate
{
    public class GetInvoiceByDateQueryHandler : IRequestHandler<GetInvoiceByDateQuery,Result<PagedResult<InvoicesDto>>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        public GetInvoiceByDateQueryHandler(IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<InvoicesDto>>> Handle(GetInvoiceByDateQuery request, CancellationToken cancellationToken)
        {
          var invoices=   await  _invoiceRepository.GetInvoiceByDate(request.Page, request.PageSize, request.date,cancellationToken);

            return Result<PagedResult<InvoicesDto>>.Success(new PagedResult<InvoicesDto>
            {
             
                Items = _mapper.Map<List<InvoicesDto>>(invoices.Items),
                TotalCount = invoices.TotalCount,
                Page = invoices.Page,
                PageSize = invoices.PageSize,
            });
            
        }
    }
}
