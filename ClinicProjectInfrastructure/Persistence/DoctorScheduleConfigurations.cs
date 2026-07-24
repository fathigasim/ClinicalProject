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
    internal class DoctorScheduleConfigurations : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(e => e.Id);

            // 1. Tell EF that the Doctor property is the navigation
            builder.HasOne(p => p.Doctor)
                   .WithMany(p=>p.DoctorSchedules)
                   .HasForeignKey(p => p.DoctorId);

            // 2. Explicitly tell EF to use your private backing field for DDD encapsulation
            builder.Navigation(p => p.Doctor)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(p => p.ScheduledDate);
        }
    }
}
