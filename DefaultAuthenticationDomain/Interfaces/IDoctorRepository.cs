
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IDoctorRepository: IRepository<Doctor>
    {
        public Task<Doctor?> GetByEmailAsync(string email, CancellationToken ct);
    }
}
