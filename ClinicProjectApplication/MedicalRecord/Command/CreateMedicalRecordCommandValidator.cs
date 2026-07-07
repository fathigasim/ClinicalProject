using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.MedicalRecord.Command
{
    public class CreateMedicalRecordCommandValidator:AbstractValidator<CreateMedicalRecordCommand>
    {
        public CreateMedicalRecordCommandValidator()
        {
            RuleFor(x => x.AppointmentNumber).NotEmpty().WithMessage("Appointment number is required.");
            RuleFor(x => x.Diagnosis).NotEmpty().WithMessage("Diagnosis is required.").MaximumLength(100).WithMessage("Diagnosis cannot exceed 100 characters.");
            
        }
    }
}
