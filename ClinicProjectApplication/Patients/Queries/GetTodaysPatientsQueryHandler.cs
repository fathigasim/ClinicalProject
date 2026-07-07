using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Patients.Dto;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Patients.Queries
{
    public class GetTodaysPatientsQueryHandler : IRequestHandler<GetTodaysPatientsQuery, Result<List<PatientDto>>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        public GetTodaysPatientsQueryHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }
        public async Task<Result<List<PatientDto>>> Handle(GetTodaysPatientsQuery request, CancellationToken cancellationToken)
        {
           var todaysPatients= await  _patientRepository.GetTodaysPatients(cancellationToken);
            if (todaysPatients.Count() <= 0)
            {
             return   Result<List<PatientDto>>.Failure("No patients registered today register first");
            }
            var todaysPatientsDto = _mapper.Map<List<PatientDto>>(todaysPatients);
         
            return Result<List<PatientDto>>.Success(todaysPatientsDto);
        }
    }
}
