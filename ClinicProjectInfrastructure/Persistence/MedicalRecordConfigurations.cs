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
    internal class MedicalRecordConfigurations : IEntityTypeConfiguration<MedicalRecords>
    {
        public void Configure(EntityTypeBuilder<MedicalRecords> builder)
        {


            builder.HasKey(p => p.Id);
            builder.HasOne(a => a.Appointment).WithOne()
                .HasForeignKey<MedicalRecords>(r => r.AppointmentId);

      
        }
    }
}
