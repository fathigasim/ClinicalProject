using ClinicProjectApplication.Common;
using ClinicProjectApplication.MedicalRecord.Dtos;
using ClinicProjectDomain.Common.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Queries.GetAllMedicalRecords
{
    public record GetAllMedicalRecordsQuery(int page,int pageSize) : IRequest<Result<PagedResult<MedicalRecordDto>>>
    {
    }
}
