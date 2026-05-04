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
    public class WeeklyScheduleRepository : Repository<WeeklySchedule>, IWeeklyScheduleRepository
    {
        private readonly IReadDbContext _readDbContext;
        public WeeklyScheduleRepository(AppDbContext context, IReadDbContext readDbContext) : base(context)
        {
            _readDbContext = readDbContext;
        }
        public async Task<bool> IsDoctorScheduledToday(Guid doctorId,DayOfWeek dayofweek, CancellationToken ct)
        {
            // Khartoum is UTC+2
            var khartoumOffset = TimeSpan.FromHours(2);
            var khartoumTime = DateTimeOffset.UtcNow.ToOffset(khartoumOffset);


            return await _readDbContext.ReadSet<WeeklySchedule>()
                .AnyAsync(ws => ws.DoctorId == doctorId && ws.DayOfWeek == dayofweek, ct);
        }

        public async Task<bool> HasOverlappingSchedule(
    Guid doctorId,
    DayOfWeek day,
    TimeOnly start,
    TimeOnly end,
    CancellationToken ct)
        {
            return await _readDbContext.ReadSet<WeeklySchedule>()
                .AnyAsync(ws =>
                    ws.DoctorId == doctorId &&
                    ws.DayOfWeek == day &&
                    start < ws.EndTime &&
                    ws.StartTime < end,
                    ct);
        }

        
    }
}
