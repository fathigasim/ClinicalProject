
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ClinicProjectInfrastructure.Persistence
{
    // Infrastructure/Persistence/AppDbContext.cs
   public class AppDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole, string>
    ,IReadDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options
          )  
           : base(options)
        {
        }
        public class AppDbContextFactory
       : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var basePath = Directory.GetCurrentDirectory();

                var config = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DefaultAuthenticationApi"))
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = config.GetConnectionString("DefaultConnection");

                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                return new AppDbContext(optionsBuilder.Options);
            }
        }
        public IQueryable<T> ReadSet<T>() where T : class
         => Set<T>().AsNoTracking();

        protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<ApplicationUser>()
         .OwnsMany(u => u.RefreshTokens, rt =>
         {
             rt.WithOwner().HasForeignKey(t => t.UserId);
             rt.HasKey(t => t.Id);
             rt.Property(t => t.Token).IsRequired().HasMaxLength(128);
             rt.HasIndex(t => t.Token).IsUnique();
             rt.Property(t => t.Expires).IsRequired();
         });
            b.Entity<Doctor>()
             .HasKey(d => d.Id);
            b.Entity<Doctor>()
             .Property(d => d.FirstName).IsRequired().HasMaxLength(50);
            b.Entity<Doctor>()
             .Property(d => d.LastName).IsRequired().HasMaxLength(50);
            b.Entity<Doctor>()
             .Property(d => d.Specialization).IsRequired().HasMaxLength(100);
            b.Entity<Doctor>()
             .Property(d => d.Phone).IsRequired().HasMaxLength(20);
            b.Entity<Doctor>()
             .Property(d => d.Email).IsRequired().HasMaxLength(100);
            //appointment configuration
            
            b.Entity<Appointment>(p =>
            {
                p.HasKey(p => p.Id);
                p.HasOne(p => p.Patient).WithMany(p => p.Appointments).HasForeignKey(p => p.PatiendId);
                p.HasOne(p => p.Doctor).WithMany(p => p.Appointments).HasForeignKey(p => p.DoctorId);
            });
           

            b.Entity<MedicalRecords>(p =>
            {
                p.HasKey(p => p.Id);
                p.HasOne(p => p.Appointment).WithOne(p => p.MedicalRecord).HasForeignKey<MedicalRecords>(r=>r.AppointmentId);

            });
            b.Entity<Invoices>(p =>
            {
                p.HasKey(p => p.Id);
                p.HasOne(p => p.Appointment).WithOne(p => p.Invoices).HasForeignKey<Invoices>(i => i.AppointmentId);
            });
            b.Entity<Payments>(p =>
                {
                    p.HasKey(p => p.Id);
                    p.HasOne(p => p.Invoice).WithOne(i => i.Payments).HasForeignKey<Payments>(p => p.InvoiceId);
                });
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecords> MedicalRecords { get; set; }
        public DbSet<Invoices> Invoices { get; set; }

        public DbSet<Payments> Payments { get; set; }

    }
}
