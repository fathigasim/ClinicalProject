
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Patients.Queries
{
    public record GetPatientByPhoneQuery(string Phone) : IRequest<Patient>, ICacheableQuery
    {
        public string CacheKey => $"Patient{Phone}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(15);

        public bool BypassCache => throw new NotImplementedException();
    }
}
