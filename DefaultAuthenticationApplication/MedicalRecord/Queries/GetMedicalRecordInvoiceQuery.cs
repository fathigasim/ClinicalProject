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
    public record GetMedicalRecordInvoiceQuery : IRequest<Result<List<MedicalInvoiceDto>>> 
    {
    }
}
