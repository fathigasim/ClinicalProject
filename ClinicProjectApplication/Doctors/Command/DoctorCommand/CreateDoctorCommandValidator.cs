using FluentValidation;


namespace ClinicProjectApplication.Doctors.Command.DoctorCommand
{
    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Command cannot be null.");
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First name is required.")
                    .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
                RuleFor(x => x.Specialization).NotEmpty().WithMessage("Specialization is required.")
                    .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.")
                .MinimumLength(13).WithMessage("Phone number cannot be less than 13 characters");
                RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Please select gender.");
        }
    }
}
