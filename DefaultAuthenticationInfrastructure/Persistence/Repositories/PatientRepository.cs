using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;


namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        private readonly IReadDbContext _readDbContext;
        public PatientRepository(AppDbContext context,IReadDbContext readDbContext) : base(context)
        {
            _readDbContext = readDbContext;
        }

        public Task<Patient?> GetByPhone(string phone, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetTodaysPatients( CancellationToken ct)
        {
            var today=DateTime.Now;
          var todayPatients= await _readDbContext.ReadSet<Patient>().Where(p => p.CreatedAt.Date == today.Date).Select(p=>new Patient() {Id=p.Id,FirstName=p.FirstName,LastName=p.LastName}).ToListAsync(ct);
            return todayPatients;
        }


      
    }
}
