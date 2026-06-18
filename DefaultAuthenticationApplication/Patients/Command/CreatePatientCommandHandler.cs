using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Patients.Command
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand,Result< string>>
    {
       private readonly IPatientRepository _patientRepository;
        
        public CreatePatientCommandHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<Result<string>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient
            {

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                DOB = request.DOB,
                Gender = request.Gender,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow,
            };
            await _patientRepository.AddAsync(patient);
            return Result<string>.Success($"patient {patient.FirstName} {patient.LastName} added successfully");
        }

    }
}
