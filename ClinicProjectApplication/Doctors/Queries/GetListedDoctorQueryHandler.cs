using AutoMapper;
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
    public class GetListedDoctorQueryHandler (IDoctorRepository doctorRepository,IMapper mapper) : IRequestHandler<GetListedDoctorQuery, List<DoctorDto>>
    {
       
        public async Task<List<DoctorDto>> Handle(GetListedDoctorQuery request, CancellationToken cancellationToken)
        {
         var doctors=   await doctorRepository.GetListedDoctorsAsync(cancellationToken);
            if (doctors.Any())
            {
              var doctorsDto=  mapper.Map<List<DoctorDto>>( doctors);
                return doctorsDto;
            }


            return new List<DoctorDto>();
        }
    }
}
