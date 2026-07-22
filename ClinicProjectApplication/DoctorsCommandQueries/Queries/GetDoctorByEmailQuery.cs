using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public record GetDoctorByEmailQuery(string Email) : IRequest<ClinicProjectDomain.Entities.Doctor>;
   
}
