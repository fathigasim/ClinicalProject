using AutoMapper;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Queries
{
    public class GetWeeklyScheduleQueryHandler : IRequestHandler<GetWeeklyScheduleQuery, DoctorsScheduleDto>
    {
        private readonly IWeeklyScheduleRepository _weeklyScheduleRepo;
        private readonly IMapper _mapper;
        public GetWeeklyScheduleQueryHandler(IWeeklyScheduleRepository doctorScheduleRepo,
             IMapper mapper
            )
        {
            _weeklyScheduleRepo = doctorScheduleRepo;
            _mapper = mapper;
        }
        public async Task<DoctorsScheduleDto> Handle(GetWeeklyScheduleQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var docsWeeklySchedule = await _weeklyScheduleRepo.DoctorsScheduleDays(cancellationToken);
            var result = new DoctorsScheduleDto
            {
                Schedule = docsWeeklySchedule
                .Where(p=>p.ScheduledDate >= today &&
    p.ScheduledDate <= today.AddDays(14))
       .GroupBy(p => p.ScheduledDate).OrderBy(p => p.Key)
       .ToDictionary(
           g => g.Key,
           g => g.Select(p => new DaySchedule
           {
               DoctorId = p.DoctorId,
               DoctorName = p.Doctor.FirstName +" "+p.Doctor.LastName,
               ScheduleDate=p.ScheduledDate,
               StartTime = p.StartTime,
               EndTime = p.EndTime,
               SlotDurationMinutes = p.SlotDurationMinutes,
               IsActive = p.IsActive
           }).ToList()
       )
            };
            return result;
        }
    }
}
