using ClinicProjectDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure
{
    internal class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {

            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName).IsRequired().HasMaxLength(50);
                builder.Property(d => d.LastName).IsRequired().HasMaxLength(50);
                builder.Property(d => d.Specialization).IsRequired().HasMaxLength(100);
                builder.Property(d => d.Phone).IsRequired().HasMaxLength(20);
                builder.Property(d => d.Email).IsRequired().HasMaxLength(100);
                builder.HasMany(d => d.WeeklySchedules).WithOne(a => a.Doctor).HasForeignKey(a => a.DoctorId);
            
        }
    }
}
