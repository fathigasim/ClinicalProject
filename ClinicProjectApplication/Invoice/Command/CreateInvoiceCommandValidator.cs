using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Invoice.Command
{
    public class CreateInvoiceCommandValidator:AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(x => x.AppointmentNo).NotEmpty().WithMessage("Please Select Appointment No")
                .MaximumLength(20).WithMessage("Character entered should not exceed 20");
            RuleFor(x => x.TotalAmount).NotEmpty().WithMessage("Please select amount")
                .GreaterThan(0).WithMessage("Amount cannot be less than or equal to 0");
        }
    }
}
