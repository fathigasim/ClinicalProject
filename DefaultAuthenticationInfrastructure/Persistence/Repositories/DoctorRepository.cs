
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;


namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class DoctorRepository : Repository<Doctor>,IDoctorRepository
    {
        public DoctorRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Doctor?> GetByEmailAsync(string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
