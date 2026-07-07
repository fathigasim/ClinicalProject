
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Patients.Queries;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAuthenticationApplication.Patients.Queries
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