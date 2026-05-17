using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;


namespace ClinicProjectDomain.Interfaces
{
    public interface IMedicalRecordRepository : IRepository<MedicalRecords>
    {
        Task<PagedResult<MedicalRecords>?> GetAllPatientsMedicalReocrd(int page, int pageSize, CancellationToken ct);
        Task<MedicalRecords?> PatientMedicalRecord(Guid patientId);

        
    }
}
