
using ClinicProjectApplication.Common;

using ClinicProjectApplication.PatientsCommandQueries.Queries;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace DefaultAuthenticationApplication.PatientsCommandQueries.Queries
{
    public class GetPatientByNameQueryHandler : IRequestHandler<GetPatientByNameQuery, Result<PagedResult<Patient?>>>
    {
        private readonly IPatientRepository _patientRepository;
        public GetPatientByNameQueryHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<Result<PagedResult<Patient?>>> Handle(GetPatientByNameQuery request, CancellationToken cancellationToken)
        {
          
            var pagedPatient=  await _patientRepository.GetByQuery(request.q,request.page,request.pageSize,cancellationToken);
           
            return Result<PagedResult<Patient?>>.Success(pagedPatient);
        }
    }
}