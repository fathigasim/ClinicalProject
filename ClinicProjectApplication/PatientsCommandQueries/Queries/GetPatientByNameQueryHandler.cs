
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Common.Services;
using ClinicProjectApplication.PatientsCommandQueries.Queries;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace DefaultAuthenticationApplication.PatientsCommandQueries.Queries
{
    public class GetPatientByNameQueryHandler : IRequestHandler<GetPatientByNameQuery, Result<PagedResult<Patient?>>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ICacheService _cacheService;
        public GetPatientByNameQueryHandler(IPatientRepository patientRepository, ICacheService cacheService)
        {
            _patientRepository = patientRepository;
            _cacheService = cacheService;
        }
        public async Task<Result<PagedResult<Patient?>>> Handle(GetPatientByNameQuery request, CancellationToken cancellationToken)
        {
          
            var pagedPatient=  await _patientRepository.GetByQuery(request.q,request.page,request.pageSize,cancellationToken);
            _cacheService.Set(request.CacheKey, request.Prefix, pagedPatient, request.Expiration ?? TimeSpan.FromMinutes(5));
            return Result<PagedResult<Patient?>>.Success(pagedPatient);
        }
    }
}