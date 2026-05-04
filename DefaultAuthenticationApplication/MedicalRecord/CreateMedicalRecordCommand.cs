
using ClinicProjectApplication.Interfaces;
using MediatR;


namespace ClinicProjectApplication.MedicalRecord
{
    public record CreateMedicalRecordCommand(Guid AppointmentId ,string Diagnosis, string Notes) : IRequest<Guid>,ITransactionalRequest;
  
}
