
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Common.Pagination;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Extensions;
using ClinicProjectInfrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class MedicalRecordRepository: Repository<MedicalRecords>, IMedicalRecordRepository
    {
        private readonly IReadDbContext _readDbContext;
        public MedicalRecordRepository(AppDbContext context, IReadDbContext readDbContext) : base(context)
        {
            _readDbContext = readDbContext;
        }


        public async Task<PagedResult<MedicalRecords>?> GetAllPatientsMedicalReocrd(int page,int pageSize,CancellationToken ct)
        {
            var medicalRecord = await _readDbContext.ReadSet<MedicalRecords>()
                                  .WithIncludes(p => p.Include(pre=>pre.Prescriptions)
                                  .ThenInclude(it=>it.PrescriptionItems))
                                  
                                  .OrderByDescending(p => p.CreatedAt)
                                  .ToPagedAsync(page, pageSize, ct);
            return medicalRecord;
        }
        public async Task<MedicalRecords?> PatientMedicalRecord(Guid patientId)
        {
            var patientMedicalRecord= await _readDbContext.ReadSet<MedicalRecords>()
                .Include(p=>p.Appointment)
                .ThenInclude(p=>p.Patient)
                .Where(p => p.Appointment.Patient.Id == patientId)
                
                .FirstOrDefaultAsync();
            return patientMedicalRecord;
        }

        



    }
}
