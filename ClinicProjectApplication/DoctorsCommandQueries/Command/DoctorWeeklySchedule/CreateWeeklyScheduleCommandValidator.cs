using ClinicProjectDomain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.DoctorsCommandQueries.Command.DoctorWeeklySchedule
{
    public class CreateWeeklyScheduleCommandValidator : AbstractValidator<CreateWeeklyScheduleCommand>
    {
        public CreateWeeklyScheduleCommandValidator()
        {
            RuleFor(p => p).Must(cmd => DoctorSchedule.IsTimeSlotValid(cmd.ScheduleDate,cmd.StartTime))
                 .WithMessage("The selected time slot is in the past."); ;
            RuleFor(x => x.DoctorId)
           .NotNull().WithMessage("Doctor is required.")
           .NotEqual(Guid.Empty).WithMessage("Please select a valid doctor.");

            RuleFor(x => x.ScheduleDate)
                .NotEqual(default(DateOnly)).WithMessage("Schedule date is required.");

            RuleFor(x => x.StartTime)
                .NotEqual(default(TimeOnly)).WithMessage("Start time is required.");
                
            RuleFor(x => x.EndTime)
                .NotEqual(default(TimeOnly)).WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");
            RuleFor(x => x.ScheduleDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Please Enter valid date");
        }

        
    }
}
