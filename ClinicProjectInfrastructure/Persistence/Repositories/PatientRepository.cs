using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Extensions;
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

        public async Task<PagedResult<Patient?>> GetByQuery(string q,int page,int pageSize, CancellationToken ct)
        {
            var search = q?.ToLower();
         return   await _readDbContext.ReadSet<Patient>().Where(p =>
         string.IsNullOrEmpty(q)||
         p.FirstName.ToLower().Contains(search) ||
            p.LastName.ToLower().Contains(search) ||
            p.Phone.Contains(search)).ToPagedAsync(page, pageSize, ct);
        }

        public async Task<List<Patient>> GetTodaysPatients(CancellationToken ct)
        {
         return   await _readDbContext.ReadSet<Patient>().Where(p =>p.CreatedAt.Date
           == DateTime.UtcNow.Date).ToListAsync(ct);
        }

        //public async Task<List<Patient>> GetTodaysPatients( CancellationToken ct)
        //{
        //    var today=DateTime.Now;
        //  var todayPatients= await _readDbContext.ReadSet<Patient>().Where(p => p.CreatedAt.Date == today.Date).Select(p=>new Patient() {Id=p.Id,FirstName=p.FirstName,LastName=p.LastName}).ToListAsync(ct);
        //    return todayPatients;
        //}



    }
}
