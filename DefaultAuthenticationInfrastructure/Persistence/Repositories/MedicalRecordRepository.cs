
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
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

        public async Task<MedicalRecords?> PatientMedicalRecord(Guid patientId)
        {
            var patientMedicalRecord= await _readDbContext.ReadSet<MedicalRecords>()
                .Include(p=>p.Appointment)
                .ThenInclude(p=>p.Patient)
                .AsNoTracking()
                .Where(p => p.Appointment.Patient.Id == patientId)
                .FirstOrDefaultAsync();
            return patientMedicalRecord;
        }

        



    }
}
