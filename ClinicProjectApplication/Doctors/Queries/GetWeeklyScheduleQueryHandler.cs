using AutoMapper;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetWeeklyScheduleQueryHandler : IRequestHandler<GetWeeklyScheduleQuery, DoctorsWeeklyScheduleDto>
    {
        private readonly IWeeklyScheduleRepository _weeklyScheduleRepo;
        private readonly IMapper _mapper;
        public GetWeeklyScheduleQueryHandler(IWeeklyScheduleRepository weeklyScheduleRepo,
             IMapper mapper
            )
        {
            _weeklyScheduleRepo = weeklyScheduleRepo;
            _mapper = mapper;
        }
        public async Task<DoctorsWeeklyScheduleDto> Handle(GetWeeklyScheduleQuery request, CancellationToken cancellationToken)
        {
            var docsWeeklySchedule = await _weeklyScheduleRepo.DoctorsScheduleDays(cancellationToken);
            var result = new DoctorsWeeklyScheduleDto
            {
                Schedule = docsWeeklySchedule
       .GroupBy(p => p.DayOfWeek)
       .ToDictionary(
           g => g.Key,
           g => g.Select(p => new DaySchedule
           {
               DoctorId = p.DoctorId,
               DoctorName = p.Doctor.FirstName +" "+p.Doctor.LastName,
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
