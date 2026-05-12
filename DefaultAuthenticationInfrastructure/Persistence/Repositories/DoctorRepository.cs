
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class DoctorRepository : Repository<Doctor>,IDoctorRepository
    {
        private readonly IReadDbContext _readDbContext;
        public DoctorRepository(AppDbContext context, IReadDbContext readDbContext) : base(context)
        {
            _readDbContext = readDbContext;
        }
        public async Task<WeeklySchedule?> DoctorWeeklySchedule(Guid doctorId, DayOfWeek dayOfWeek , CancellationToken cancellationToken)
        {
               return await  _readDbContext.ReadSet<WeeklySchedule>()
                .Where(ws => ws.DoctorId == doctorId && ws.DayOfWeek==dayOfWeek)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Doctor>?> DoctorsTodaySchedule(CancellationToken cancellationToken)
        {
            var today = DateTime.Today.DayOfWeek;
            return await _readDbContext.ReadSet<WeeklySchedule>().Include(p=>p.Doctor)
             .Where(ws=>ws.DayOfWeek == today)
             .Select(p=>new Doctor { Id = p.DoctorId, FirstName = p.Doctor.FirstName,
                 LastName=p.Doctor.LastName,Specialization=p.Doctor.Specialization }).ToListAsync(cancellationToken);
        }
    }
}
