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
            // Implement your logic here
            var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
            if (doctor == null)
            {
                return Result<string>.Failure("Doctor not found.");
            }
             var weeklyScheduleDto= new WeeklyScheduleDto() { 
                DoctorId = request.DoctorId,
                DayOfWeek = request.DayOfWeek,
                
                 StartTime = request.StartTime.TimeOfDay,
                EndTime = request.EndTime.TimeOfDay
             };
            var weeklySchedule = _mapper.Map<WeeklySchedule>(weeklyScheduleDto);
            var isHoliday = weeklySchedule.IsHoliday(weeklyScheduleDto.DayOfWeek);
            if(isHoliday)
            {
                return Result<string>.Failure("Cannot create schedule on a holiday.");
            }
            var isDoctorBookedToday=   await _weeklyScheduleRepository.IsDoctoryScheduledToday(weeklySchedule.DoctorId, weeklyScheduleDto.DayOfWeek, cancellationToken);
                if(isDoctorBookedToday)
                {
                    return Result<string>.Failure("Doctor is already scheduled for today.");
            }

            await  _weeklyScheduleRepository.AddAsync(weeklySchedule);
            

            return Result<string>.Success("Weekly schedule created successfully.");
        }
    }
}
