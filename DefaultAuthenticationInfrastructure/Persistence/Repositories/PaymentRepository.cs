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
    public class PaymentRepository :Repository<Payments>,IPaymentRepository
    {
        public PaymentRepository(AppDbContext context):base(context)
        {
            
        }


    }
}
