using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using MediatR;


namespace ClinicProjectApplication.MedicalRecord.Command
{
    public record CreateMedicalRecordCommand
        (string AppointmentNumber ,string Diagnosis,string MedicationName
        ,string Dosage, int Frequency, DateTime Duration) 
        : IRequest<Result<string>>,ITransactionalRequest;

                         
                           
}
