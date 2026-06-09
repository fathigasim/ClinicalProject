using ClinicProjectDomain.Common.Pagination;
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
        Task<PagedResult<Patient?>> GetByQuery(string q, int page, int pageSize, CancellationToken ct);
        Task<List<Patient>> GetTodaysPatients(CancellationToken ct);
    }
}
