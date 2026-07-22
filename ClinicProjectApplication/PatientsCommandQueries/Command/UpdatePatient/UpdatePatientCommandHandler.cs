using AutoMapper;
using ClinicProjectApplication.Appointments.AppointmentCommand;

using ClinicProjectApplication.PatientsCommandQueries.Command.UpdatePatient;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;


namespace ClinicProjectApplication.PatientsCommandQueries.Command.UpdatePatient
{
    public class UpdatePatientCommandHandler(IRepository<Patient> repository,IMapper mapper) : IRequestHandler<UpdatePatientCommand,string>
    {

      
        public async Task<string> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient=await repository.GetByIdAsync(request.Id);
            if (patient == null)
            {
                throw new KeyNotFoundException(nameof(request.Id));
            }
            patient.Update(request.FirstName, request.LastName, request.Email, request.DOB, request.Phone,request.Gender);
           // repository.Update(patient);
                 return "update successfully";
        }
    }
}
