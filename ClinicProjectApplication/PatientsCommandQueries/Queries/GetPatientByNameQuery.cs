
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using MediatR;


namespace ClinicProjectApplication.PatientsCommandQueries.Queries
{
    public record GetPatientByNameQuery(string? q,int page,int pageSize) : IRequest<Result<PagedResult<Patient?>>>, ICacheableQuery
    {
        //public string CacheKey => $"Patient{q}-{page}-{pageSize}";
        // Key format: patient:list:q=john:page=1:size=10
        public string Prefix => "patient-list";
        public string CacheKey => $"patient:list:q={q ?? "all"}:page={page}:size={pageSize}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(15);

        public bool BypassCache => throw new NotImplementedException();
    }
}
