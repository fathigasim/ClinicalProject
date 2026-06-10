using ClinicProjectDomain.Entities;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Doctors.Command.DoctorWeeklySchedule
{
    public class CreateWeeklyScheduleCommandValidator : AbstractValidator<CreateWeeklyScheduleCommand>
    {
        public CreateWeeklyScheduleCommandValidator()
        {
             RuleFor(p=>p).Must(cmd => WeeklySchedule.IsTimeSlotValid(cmd.scheduleDate, cmd.startTime))
    .WithMessage("The selected time slot is in the past.");
        }

        
    }
}
