
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Patients.Queries
{
    public record GetPatientByNameQuery(string? q,int page,int pageSize) : IRequest<Result<PagedResult<Patient?>>>, ICacheableQuery
    {
        public string CacheKey => $"Patient{q}-{page}-{pageSize}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(15);

        public bool BypassCache => throw new NotImplementedException();
    }
}
