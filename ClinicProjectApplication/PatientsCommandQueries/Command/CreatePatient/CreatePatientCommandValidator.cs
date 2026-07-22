using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.PatientsCommandQueries.Command.CreatePatient
{
    public class CreatePatientCommandValidator:AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Command cannot be null.");
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.DOB).NotEmpty().Must(p => p.Date.Date <= DateTime.Now.Date.AddYears(-15)).WithMessage("Not Adult Patient");
                RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+[1-9]\d{7,14}$")
    .WithMessage("Phone must be in international format like +123456789");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
        }
    }
}
