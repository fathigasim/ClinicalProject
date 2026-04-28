using ClinicProjectDomain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IPatientRepository:IRepository<Patient>
    {
        Task<Patient?> GetByPhone(string phone, CancellationToken ct);
    }
}
