using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Interfaces;
using MediatR;

namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetAllDoctorQueryHandler : IRequestHandler<GetAllDoctorQuery, Result<List<DoctorDto>>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        public GetAllDoctorQueryHandler(IDoctorRepository doctorRepository,IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }
        public async Task<Result<List<DoctorDto>>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctors=await _doctorRepository.GetAllAsync(cancellationToken);
           var doctorsDto=   _mapper.Map<List<DoctorDto>>(doctors);

            return  Result<List<DoctorDto>>.Success(doctorsDto);
        }
    }
}
