using ClinicProjectDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence
{
    internal class PaymentConfigurations : IEntityTypeConfiguration<Payments>
    {
        public void Configure(EntityTypeBuilder<Payments> builder)
        {
           
                builder.HasKey(p => p.Id);
                builder.HasOne(p => p.Invoice).WithOne(i => i.Payment).HasForeignKey<Payments>(p => p.InvoiceId);
            
        }
    }
}
