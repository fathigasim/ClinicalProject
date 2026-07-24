using ClinicProjectApplication.Common;
using ClinicProjectApplication.DoctorsCommandQueries.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public record GetDoctorScheduleByIdQuery(Guid id) : IRequest<Result<DoctorScheduleDto>>;
   
}
