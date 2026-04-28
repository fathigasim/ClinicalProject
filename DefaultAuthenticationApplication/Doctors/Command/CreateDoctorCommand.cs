
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Command
{
    public record CreateDoctorCommand
        (string FirstName, string LastName, string Specialization, string Phone, string Email)
        :IRequest<Guid>,ITransactionalRequest;
   
}
