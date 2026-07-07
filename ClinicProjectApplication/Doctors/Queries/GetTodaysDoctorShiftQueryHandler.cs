using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetTodaysDoctorShiftQueryHandler : IRequestHandler<GetTodaysDoctorShiftQuery, Result<List<DoctorDto>>>
    {
        private readonly IDoctorRepository _doctoryRepository;
        private readonly IMapper _mapper;
        public GetTodaysDoctorShiftQueryHandler(IDoctorRepository doctoryRepository, IMapper mapper)
        {
         _doctoryRepository = doctoryRepository;   
            _mapper = mapper;
        }
        public async Task<Result<List<DoctorDto>>> Handle(GetTodaysDoctorShiftQuery request, CancellationToken cancellationToken)
        {
            var scheduleDoctors=await _doctoryRepository.DoctorsTodaySchedule(cancellationToken);
            var scheduleDoctorsDto=  _mapper.Map<List<DoctorDto>>(scheduleDoctors);
            if (scheduleDoctorsDto.Any())
            {
                return Result<List<DoctorDto>>.Success(scheduleDoctorsDto);
            }
            return Result<List<DoctorDto>>.Failure("Doctors not available today");
        }
    }
}
