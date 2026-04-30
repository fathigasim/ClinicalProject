using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.Doctors.Command.DoctorCommand
{
    public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Guid>
    {
       private readonly IDoctorRepository _doctorRepository;
        
        public CreateDoctorCommandHandler(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }
        public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {

           var doctor = new ClinicProjectDomain.Entities.Doctor
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Specialization = request.Specialization,
                Phone = request.Phone,
                Email = request.Email
            };
  await _doctorRepository.AddAsync(doctor);
            return doctor.Id;
        }

    }
}
