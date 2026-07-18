
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Extensions;
using ClinicProjectInfrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public Task<PagedResult<Doctor>> GetAllDoctorsAsync(int page, int pageSize,CancellationToken ct) =>
    _readDbContext.ReadSet<Doctor>()
     
      .OrderBy(c => c.CreatedAt)
      .ToPagedAsync(page, pageSize,ct);
        public async Task<WeeklySchedule?> DoctorWeeklySchedule(Guid doctorId, DayOfWeek dayofWeek , CancellationToken cancellationToken)
        {
               return await  _readDbContext.ReadSet<WeeklySchedule>()
                .Where(ws => ws.DoctorId == doctorId && 
                
                 ws.DayOfWeek==dayofWeek)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<WeeklySchedule?> DoctorScheduleDate(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            return await _readDbContext.ReadSet<WeeklySchedule>()
             .Where(ws => ws.DoctorId == doctorId && ws.ScheduledDate == date)
             .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Doctor>?> DoctorsSchedule(CancellationToken cancellationToken)
        {
            var today = DateTime.Today.DayOfWeek;
            var doctors= await _readDbContext.ReadSet<WeeklySchedule>()
                .Include(p=>p.Doctor).GroupBy(d=>d.Doctor.Id)
                
        
             .Select(p=>new Doctor { Id = p.Key, FirstName = p.First().Doctor.FirstName,
                 LastName=p.First().Doctor.LastName,Specialization=p.First().Doctor.Specialization })
             .ToListAsync(cancellationToken);
            return doctors;
        }

        public async Task<List<Doctor>?> DoctorsTodaySchedule(CancellationToken cancellationToken)
        {
            var today = DateTime.Today.DayOfWeek;
            var doctors = await _readDbContext.ReadSet<WeeklySchedule>()
                .Include(p => p.Doctor)
             .Select(p => new Doctor
             {
                 Id = p.DoctorId,
                 FirstName = p.Doctor.FirstName,
                 LastName = p.Doctor.LastName,
                 Specialization = p.Doctor.Specialization
             })
             .ToListAsync(cancellationToken);

            return doctors;
        }

        public async Task<WeeklySchedule?> DoctorSchedule(Guid doctorId, DateOnly scheduleDate, CancellationToken cancellationToken)
        {
         return   await _readDbContext.ReadSet<WeeklySchedule>().Where
              (p => p.DoctorId == doctorId && p.ScheduledDate == scheduleDate).FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<List<Doctor>> GetListedDoctorsAsync(CancellationToken ct)
        {
            var today = DateOnly.FromDateTime( DateTime.UtcNow.Date);
            var weekFromNow = today.AddDays(7);
            return     await _readDbContext.ReadSet<WeeklySchedule>()
                  .Where(p => p.ScheduledDate >= today && p.ScheduledDate < weekFromNow)
                .GroupBy(p=>p.DoctorId)
                
                .Select(p => new Doctor { Id=p.Key,FirstName=p.First().Doctor.FirstName
                ,LastName=p.First().Doctor.LastName
                ,Specialization=p.First().Doctor.Specialization}).ToListAsync();
        }
    }
}
