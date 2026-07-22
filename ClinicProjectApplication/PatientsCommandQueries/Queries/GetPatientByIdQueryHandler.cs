using AutoMapper;
using ClinicProjectApplication.PatientsCommandQueries.Dto;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PatientsCommandQueries.Queries
{
    public class GetPatientByIdQueryHandler(IRepository<Patient> repository,IMapper mapper) : IRequestHandler<GetPatientByIdQuery,PatientDto>
    {
        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
           var patient= await repository.GetByIdAsync(request.id, cancellationToken);
           return mapper.Map<PatientDto>(patient);
        }
    }
}
