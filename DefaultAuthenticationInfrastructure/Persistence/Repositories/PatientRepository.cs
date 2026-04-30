using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;


namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        
        public PatientRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Patient?> GetByPhone(string phone, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


      
    }
}
