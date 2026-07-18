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
    internal class PrescriptionConfigurations : IEntityTypeConfiguration<Prescriptions>
    {
        public void Configure(EntityTypeBuilder<Prescriptions> builder)
        {


            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.MedicalRecord).WithOne(m => m.Prescription).HasForeignKey<Prescriptions>(p => p.MedicalRecordId);
            builder.HasMany(p => p.PrescriptionItems).WithOne(i => i.Prescription).HasForeignKey(i => i.PrescriptionId);
            
        }
    }
}
