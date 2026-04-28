using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Patients.Command
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Guid>
    {
       private readonly IPatientRepository _patientRepository;
        
        public CreatePatientCommandHandler(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
           var patient = new Patient
            {
                
                FirstName = request.FirstName,
                LastName = request.LastName,
                DOB = request.DOB,
                Gender = request.Gender,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow,
            };
            await  _patientRepository.AddAsync(patient);
            return patient.Id;
        }

    }
}
