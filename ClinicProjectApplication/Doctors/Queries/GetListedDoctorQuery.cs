using ClinicProjectApplication.Doctors.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Queries
{
    public record GetListedDoctorQuery:IRequest<List<DoctorDto>>;
   
}
