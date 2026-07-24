using AutoMapper;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Doctors.Dto;
using ClinicProjectApplication.DoctorsCommandQueries.Dto;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule
{
    public class CreateWeeklyScheduleCommandHandler : IRequestHandler<CreateDoctorScheduleCommand, Result<string>>
    {  
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorScheduleRepository _weeklyScheduleRepository;
        private readonly IMapper _mapper;
        public CreateWeeklyScheduleCommandHandler(IDoctorRepository doctorRepository, IDoctorScheduleRepository weeklyScheduleRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _weeklyScheduleRepository = weeklyScheduleRepository; 
            _mapper = mapper;
        }
        public async Task<Result<string>> Handle(CreateDoctorScheduleCommand request, CancellationToken cancellationToken)
        {
            
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId.Value);
            if (doctor == null)
            {
                return Result<string>.Failure("Doctor not found.");
            }
            // 1. Validate input
            if (request.StartTime >=request.EndTime)
                return Result<string>.Failure("Invalid time range");
              
                var doctorScheduleDto = new DoctorScheduleDto()
                {
                    DoctorId = request.DoctorId.Value,
                    DoctorName = doctor.LastName + " " + doctor.FirstName,
                    DayOfWeek = request.ScheduleDate.DayOfWeek,
                    StartTime = request.StartTime,
                    ScheduledDate=request.ScheduleDate,
                    EndTime = request.EndTime,
                    SlotDurationMinutes = 30, // Default slot duration
                    IsActive = true
                };
            // 2. Holiday check
            //if (weeklySchedule.IsHoliday(request.DayOfWeek))
            //    return Result<string>.Failure("Cannot create schedule on a holiday.");

            // 3. Overlap check
            var hasOverlap = await _weeklyScheduleRepository.HasOverlappingSchedule(
                request.DoctorId,
                request.ScheduleDate,
                request.ScheduleDate.DayOfWeek,
                request.StartTime,
                request.EndTime,
                cancellationToken);

            if (hasOverlap)
                return Result<string>.Failure("Doctor already has a conflicting schedule.");

            // 4. Save
            //var weeklySchedule = _mapper.Map<WeeklySchedule>(weeklyScheduleDto);
          var doctorSchedule=  DoctorSchedule.Create(doctorScheduleDto.DoctorId, doctorScheduleDto.StartTime, doctorScheduleDto.EndTime, doctorScheduleDto.ScheduledDate);
          //  var isHoliday = weeklySchedule.IsHoliday(weeklyScheduleDto.DayOfWeek);
            //if(isHoliday)
            //{
            //    return Result<string>.Failure("Cannot create schedule on a holiday.");
            //}
           
            //var isClinicOpened = weeklySchedule.IsClincOpened();
            //if (!isClinicOpened)
            //{
            //    return Result<string>.Failure("Clinic is closed at this time");
            //}
            var doctorhasalreadybooked=   await _weeklyScheduleRepository.IsDoctorScheduledToday(doctorSchedule.DoctorId, doctorSchedule.ScheduledDate, doctorSchedule.DayOfWeek, cancellationToken);
                if(doctorhasalreadybooked)
                {
                    return Result<string>.Failure($"Doctor is already scheduled for {doctorSchedule.ScheduledDate}.");
            }
            
        await  _weeklyScheduleRepository.AddAsync(doctorSchedule);
            

            return Result<string>.Success("Weekly schedule created successfully.");
        }
    }
}
