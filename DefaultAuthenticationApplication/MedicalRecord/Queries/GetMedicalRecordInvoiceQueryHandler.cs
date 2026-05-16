using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.MedicalRecord.Dtos;
using MediatR;

namespace ClinicProjectApplication.MedicalRecord.Queries
{
    public class GetMedicalRecordInvoiceQueryHandler : IRequestHandler<GetMedicalRecordInvoiceQuery, Result<List<MedicalInvoiceDto>>>
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        public GetMedicalRecordInvoiceQueryHandler(IInvoiceService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper=mapper;
        }
        public async Task<Result<List<MedicalInvoiceDto>>> Handle(GetMedicalRecordInvoiceQuery request, CancellationToken cancellationToken)
        {
            var medicalInvoices = await _invoiceService.PatientsMedicalRecordInvoices();
    
            return Result<List<MedicalInvoiceDto>>.Success(medicalInvoices);
        }
    }
}
