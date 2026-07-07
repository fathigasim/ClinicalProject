using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Prescription
{
    public class CreatePrescriptionCommandValidator:AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionCommandValidator()
        {
            RuleFor(x=>x.patientId).NotEmpty().WithMessage("Patient is Required");
            RuleFor(x => x.frequency).NotEmpty().WithMessage("Frequency is Required");
            RuleFor(x => x.dosage).NotEmpty().WithMessage("Dosage is Required");
        }
    }
}
