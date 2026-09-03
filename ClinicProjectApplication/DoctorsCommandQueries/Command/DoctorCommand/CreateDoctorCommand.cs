using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorCommand
{
    public record CreateDoctorCommand
        (string FirstName, string LastName, string Specialization, string Gender, string Phone, string Email)
        : IRequest<Result<string>>, ITransactionalRequest, ICacheInvalidatorCommand
    {
        public string[] CacheKeys => ["DoctorsList"];

        public string[] CachePrefixes => ["DoctorsList"];
    }
}
