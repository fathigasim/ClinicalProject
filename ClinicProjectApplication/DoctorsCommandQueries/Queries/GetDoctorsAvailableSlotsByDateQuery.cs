using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.DoctorsCommandQueries.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public record GetDoctorsAvailableSlotsByDateQuery(Guid DoctorId,DateOnly date):IRequest<List<DoctorsAvailableSlotDto>>;
   
}
