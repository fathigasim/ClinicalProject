
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;


namespace ClinicProjectApplication.MedicalRecord
{
    public record CreateMedicalRecordCommand
        (string AppointmentNumber ,string Diagnosis, string Notes,string MedicationName
        ,string Dosage, int Frequency, DateTime Duration) 
        : IRequest<Result<string>>,ITransactionalRequest;

                         
                           
}
