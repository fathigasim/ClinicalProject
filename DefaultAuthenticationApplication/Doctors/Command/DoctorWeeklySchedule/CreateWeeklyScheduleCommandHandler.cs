using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Command.DoctorWeeklySchedule
{
    public class CreateWeeklyScheduleCommandHandler : IRequestHandler<CreateWeeklyScheduleCommand, Result<string>>
    {  
        private readonly IDoctorRepository _doctorRepository;
        private readonly IWeeklyScheduleRepository _weeklyScheduleRepository;
        private readonly IMapper _mapper;
        public CreateWeeklyScheduleCommandHandler(IDoctorRepository doctorRepository, IWeeklyScheduleRepository weeklyScheduleRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _weeklyScheduleRepository = weeklyScheduleRepository; 
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(CreateWeeklyScheduleCommand request, CancellationToken cancellationToken)
        {
            
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                return Result<string>.Failure("Doctor not found.");
            }
            // 1. Validate input
            if (request.startTime >= request.endTime)
                return Result<string>.Failure("Invalid time range");
              
                var weeklyScheduleDto = new WeeklyScheduleDto()
                {
                    DoctorId = request.DoctorId,
                    DoctorName = doctor.LastName + " " + doctor.FirstName,
                    DayOfWeek = request.scheduleDate.DayOfWeek,
                    StartTime = request.startTime,
                    EndTime = request.endTime,
                    SlotDurationMinutes = 30, // Default slot duration
                    IsActive = true
                };
            // 2. Holiday check
            //if (weeklySchedule.IsHoliday(request.DayOfWeek))
            //    return Result<string>.Failure("Cannot create schedule on a holiday.");

            // 3. Overlap check
            var hasOverlap = await _weeklyScheduleRepository.HasOverlappingSchedule(
                request.DoctorId,
                request.scheduleDate.DayOfWeek,
                request.startTime,
                request.endTime,
                cancellationToken);

            if (hasOverlap)
                return Result<string>.Failure("Doctor already has a conflicting schedule.");

            // 4. Save
            var weeklySchedule = _mapper.Map<WeeklySchedule>(weeklyScheduleDto);
            var isHoliday = weeklySchedule.IsHoliday(weeklyScheduleDto.DayOfWeek);
            if(isHoliday)
            {
                return Result<string>.Failure("Cannot create schedule on a holiday.");
            }
           
            //var isClinicOpened = weeklySchedule.IsClincOpened();
            //if (!isClinicOpened)
            //{
            //    return Result<string>.Failure("Clinic is closed at this time");
            //}
            var isDoctorBookedToday=   await _weeklyScheduleRepository.IsDoctorScheduledToday(weeklySchedule.DoctorId, weeklyScheduleDto.DayOfWeek, cancellationToken);
                if(isDoctorBookedToday)
                {
                    return Result<string>.Failure("Doctor is already scheduled for today.");
            }

            await  _weeklyScheduleRepository.AddAsync(weeklySchedule);
            

            return Result<string>.Success("Weekly schedule created successfully.");
        }
    }
}
