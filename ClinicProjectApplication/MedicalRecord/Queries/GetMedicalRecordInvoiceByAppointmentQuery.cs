using ClinicProjectApplication.Common;
using ClinicProjectApplication.MedicalRecord.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Queries
{
    public record GetMedicalRecordInvoiceByAppiontmentQuery(string AppoointmentNumber) : IRequest<Result<List<MedicalInvoiceDto>>>
    {
    }
}
