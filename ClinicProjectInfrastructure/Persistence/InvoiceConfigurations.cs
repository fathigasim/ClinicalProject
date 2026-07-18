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
    internal class InvoiceConfigurations : IEntityTypeConfiguration<Invoices>
    {
        public void Configure(EntityTypeBuilder<Invoices> builder)
        {
            builder.HasOne(p => p.Appointment).WithOne(p=>p.Invoice).HasForeignKey<Invoices>(p=>p.AppointmentId);

            // 2. Explicitly map the backing field for the Appointment navigation
          //  builder.Navigation(p => p.Appointment).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
