using ClinicProjectApplication.Interfaces;
using ClinicProjectInfrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ClinicProjectInfrastructure.Services
{
    public class SequenceService : ISequenceService
    {
        private readonly AppDbContext _context;

        public SequenceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var outputParam = new Microsoft.Data.SqlClient.SqlParameter
            {
                ParameterName = "@nextNumber",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Execute the command directly
            await _context.Database.ExecuteSqlRawAsync(
                "SET @nextNumber = NEXT VALUE FOR SequenceAppointmentNumbers",
                outputParam);

            var nextNumber = (int)outputParam.Value;
            var year = DateTime.UtcNow.Year;

            return $"APP-NO-{year}-{nextNumber:D3}";
        }

        public async Task<string> GenerateInvoiceNumberAsync()
        {
            var outputParam = new Microsoft.Data.SqlClient.SqlParameter
            {
                ParameterName = "@nextNumber",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Execute the command directly
            await _context.Database.ExecuteSqlRawAsync(
                "SET @nextNumber = NEXT VALUE FOR SequenceInvoiceNumbers",
                outputParam);

            var nextNumber = (int)outputParam.Value;
            var year = DateTime.UtcNow.Year;

            return $"INV-NO-{year}-{nextNumber:D3}";
        }

        public async Task<string> GenerateMedicalNumberAsync()
        {
            var outputParam = new Microsoft.Data.SqlClient.SqlParameter
            {
                ParameterName = "@nextNumber",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            // Execute the command directly
            await _context.Database.ExecuteSqlRawAsync(
                "SET @nextNumber = NEXT VALUE FOR SequenceMedicalNumber",
                outputParam);

            var nextNumber = (int)outputParam.Value;
            var year = DateTime.UtcNow.Year;

            return $"MRN-NO-{year}-{nextNumber:D3}";
        }
    }

}
