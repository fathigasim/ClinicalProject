
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
    public class InvoiceRepository: Repository<Invoices>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context):base (context)
        {
            
        }
    }
}
