using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.MedicalRecord.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Queries
{
    public class GetMedicalRecordInvoiceByAppointmentQueryHandler : IRequestHandler<GetMedicalRecordInvoiceByAppiontmentQuery, Result<List<MedicalInvoiceDto>>>
    {
        private readonly IInvoiceService _invoiceService;
        public GetMedicalRecordInvoiceByAppointmentQueryHandler(IInvoiceService invoiceService)
        {
            _invoiceService= invoiceService;
        }
        public async Task<Result<List<MedicalInvoiceDto>>> Handle(GetMedicalRecordInvoiceByAppiontmentQuery request, CancellationToken cancellationToken)
        {
         var result=   await _invoiceService.PatientMedicalRecordInvoicesByAppointmentNumber(request.AppoointmentNumber);

            if(!result.Any())
            {
                Result<List<MedicalInvoiceDto>>.Failure("No patient data available");
            }
            return Result<List<MedicalInvoiceDto>>.Success(result);
        }
    }
}
