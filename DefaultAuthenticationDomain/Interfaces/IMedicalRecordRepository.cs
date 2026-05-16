using ClinicProjectDomain.Entities;


namespace ClinicProjectDomain.Interfaces
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecords>
    {
        Task<MedicalRecords?> PatientMedicalRecord(Guid patientId);

        
    }
}
