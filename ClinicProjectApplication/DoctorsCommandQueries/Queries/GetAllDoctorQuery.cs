using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using DefaultAuthenticationApplication.PatientsCommandQueries.Dto;
using MediatR;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public record GetAllDoctorQuery(int page, int pageSize) : IRequest<Result<PagedResult<DoctorDto>>>, ICacheableQuery
    {
        public string CacheKey => "DoctorsList";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public bool BypassCache => throw new NotImplementedException();
    }
}
