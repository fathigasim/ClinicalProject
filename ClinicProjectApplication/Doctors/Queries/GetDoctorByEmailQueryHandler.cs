
using ClinicProjectApplication.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ClinicProjectApplication.Doctors.Queries
{
    public class GetDoctorByEmailQueryHandler : IRequestHandler<GetDoctorByEmailQuery, ClinicProjectDomain.Entities.Doctor>
    {
        private readonly IReadDbContext _readDbContext;
        public GetDoctorByEmailQueryHandler(IReadDbContext readDbContext)
        {
           _readDbContext = readDbContext;   
        }



        public async Task<ClinicProjectDomain.Entities.Doctor> Handle(GetDoctorByEmailQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _readDbContext.ReadSet<ClinicProjectDomain.Entities.Doctor>()
                .FirstOrDefaultAsync(d => d.Email == request.Email, cancellationToken); 
            return doctor ?? throw new KeyNotFoundException($"Doctor with email {request.Email} not found.");
        }
    }
}