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
    public class AppointmentConfigurations : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
       
         
                builder.HasKey(p => p.Id);
            builder.HasOne(p=>p.Patient).WithMany(p => p.Appointments!).HasForeignKey(p => p.PatientId);
            builder.HasOne(d=>d.Doctor).WithMany(p => p.Appointments!).HasForeignKey(p => p.DoctorId);
            
          //  builder.Navigation(p => p.Invoice).UsePropertyAccessMode(PropertyAccessMode.Field);
            //builder.HasOne(i => i.Invoice).WithOne(p => p.Appointment).HasForeignKey<Invoices>(p=>p.AppointmentId);
         
        }
    }
}
