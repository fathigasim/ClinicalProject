using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Interfaces;
using DefaultAuthenticationApplication.PatientsCommandQueries.Dto;
using MediatR;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public class GetAllDoctorQueryHandler : IRequestHandler<GetAllDoctorQuery, Result<PagedResult<DoctorDto>>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        public GetAllDoctorQueryHandler(IDoctorRepository doctorRepository,IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<DoctorDto>>> Handle(GetAllDoctorQuery request, CancellationToken cancellationToken)
        {
            var doctors=await _doctorRepository.GetAllDoctorsAsync(request.page,request.pageSize,cancellationToken);
          // var doctorsDto=   _mapper.Map<List<DoctorDto>>(doctors);

                    return Result<PagedResult<DoctorDto>>.Success(new PagedResult<DoctorDto>
                    {
                        Items = _mapper.Map<List<DoctorDto>>(doctors.Items),
                        TotalCount=doctors.TotalCount,
                        Page=doctors.Page,
                        PageSize=doctors.PageSize,
                    });
         
        }
    }
}
