
using ClinicProjectApplication.Interfaces;
using ClinicProjectApplication.Patients.Queries;
using ClinicProjectDomain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAuthenticationApplication.Patients.Queries
{
    public class GetPatientByPhoneQueryHandler : IRequestHandler<GetPatientByPhoneQuery,Patient>
    {
        private readonly IReadDbContext _readDbContext;
        public GetPatientByPhoneQueryHandler(IReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }
        public async Task<Patient> Handle(GetPatientByPhoneQuery request, CancellationToken cancellationToken)
        {
            var doctor = await _readDbContext.ReadSet<ClinicProjectDomain.Entities.Patient>()
                .FirstOrDefaultAsync(d => d.Phone == request.Phone, cancellationToken); 
            return doctor ?? throw new KeyNotFoundException($"Patient with email {request.Phone} not found.");
        }
    }
}