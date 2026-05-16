using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Interfaces;
using MediatR;

namespace ClinicProjectApplication.Doctors.Queries
{
    public record GetAllDoctorQuery : IRequest<Result<List<DoctorDto>>>, ICacheableQuery
    {
        public string CacheKey => "DoctorsList";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public bool BypassCache => throw new NotImplementedException();
    }
}
