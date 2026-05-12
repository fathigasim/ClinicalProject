using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Appointments.AppointmentCommand
{
    public class CreateAppointmentCommandValidator:AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            
            RuleFor(x => x.DoctorId).NotEmpty().WithMessage("Doctor Cannot be empty");
            RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient Cannot be empty");
            RuleFor(x => x.Notes).NotEmpty().WithMessage("Notes Cannot be empty").
            MaximumLength(50).WithMessage("Take only short notes with maximum 50 characters");
            RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime Cannot be empty");
            RuleFor(x => x.DayOfWeek).NotEmpty().WithMessage("Day of week Cannot be empty");



        }
    }
}
