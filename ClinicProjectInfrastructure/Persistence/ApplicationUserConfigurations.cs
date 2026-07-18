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
    internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
          
  builder.OwnsMany(u => u.RefreshTokens, rt =>
   {
       rt.WithOwner().HasForeignKey(t => t.UserId);
       rt.HasKey(t => t.Id);                              // explicit PK
       rt.Property(t => t.Id).ValueGeneratedNever();      // we set it in Create()
       rt.Property(t => t.Token).IsRequired().HasMaxLength(128);
       rt.HasIndex(t => t.Token).IsUnique();
       rt.Property(t => t.Expires).IsRequired();
       rt.Ignore(t => t.IsExpired);                       // computed, not mapped
       rt.Ignore(t => t.IsActive);                        // computed, not mapped
   });
        }
    }
}
