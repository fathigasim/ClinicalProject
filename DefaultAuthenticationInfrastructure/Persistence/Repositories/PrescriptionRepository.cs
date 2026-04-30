using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : Repository<Prescriptions>, IPrescriptionRepository
    {
        private readonly AppDbContext _context;
        public PrescriptionRepository(AppDbContext context) : base(context)
        {
            {
                _context = context;
            }
        }
    }
}
