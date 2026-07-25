using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class DoctorScheduleRepository : Repository<DoctorSchedule>, IDoctorScheduleRepository
    {
        private readonly IReadDbContext _readDbContext;
        private readonly AppDbContext _appDbContext;
        public DoctorScheduleRepository(AppDbContext context, IReadDbContext readDbContext) : base(context)
        {
            _readDbContext = readDbContext;
            _appDbContext = context;
        }

        public async Task<DoctorSchedule?> DoctorsScheduleById(Guid id,CancellationToken ct)
        {
          return  await _appDbContext.DoctorSchedules.Include(p => p.Doctor)
                .Where(p => p.DoctorId.Equals(id)).FirstOrDefaultAsync(ct);
        }
        public async Task<List<DoctorSchedule>> DoctorsScheduleDays( CancellationToken ct)
        {
     

            return await _appDbContext.DoctorSchedules.Include(p=>p.Doctor)
                  .OrderBy(w=>w.ScheduledDate).ToListAsync(ct);
        }
        public async Task<bool> IsDoctorScheduledToday(Guid doctorId,DateOnly scheduleDate,DayOfWeek dayofweek, CancellationToken ct)
        {
            // Khartoum is UTC+2
            var khartoumOffset = TimeSpan.FromHours(2);
            var khartoumTime = DateTimeOffset.UtcNow.ToOffset(khartoumOffset);


            return await _readDbContext.ReadSet<DoctorSchedule>()
                 .AnyAsync(ws => ws.DoctorId == doctorId &&
              ws.ScheduledDate == scheduleDate
                && ws.DayOfWeek == dayofweek, ct);
        }

        public async Task<bool> HasOverlappingSchedule(
    Guid? doctorId,
     DateOnly scheduleDate,
    DayOfWeek day,
   
    TimeOnly start,
    TimeOnly end,
    CancellationToken ct)
        {
            return await _readDbContext.ReadSet<DoctorSchedule>()
                .AnyAsync(ws =>
                    ws.DoctorId == doctorId &&
                    ws.ScheduledDate == scheduleDate &&
                    ws.DayOfWeek == day &&
                    start < ws.EndTime &&
                    ws.StartTime < end,
                    ct);
        }

        
    }
}
