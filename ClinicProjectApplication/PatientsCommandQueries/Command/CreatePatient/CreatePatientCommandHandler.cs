using ClinicProjectApplication.Common;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.PatientsCommandQueries.Command.CreatePatient
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
            var patient = Patient.Create(request.FirstName, request.LastName, request.Email, request.DOB, request.Phone, request.Gender);
           
            await _patientRepository.AddAsync(patient);
            return Result<string>.Success($"patient {patient.FirstName} {patient.LastName} added successfully");
        }

    }
}
