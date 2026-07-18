
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor? _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options,
          IHttpContextAccessor? httpContextAccessor)  
           : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
       // public class AppDbContextFactory
       //: IDesignTimeDbContextFactory<AppDbContext>
       // {
       //     public AppDbContext CreateDbContext(string[] args)
       //     {
       //         var basePath = Directory.GetCurrentDirectory();

       //         var config = new ConfigurationBuilder()
       //            //// .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DefaultAuthenticationApi"))
       //             .AddJsonFile("appsettings.json", optional: false)
       //             .Build();

       //         var connectionString = config.GetConnectionString("DefaultConnection");

       //         var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
       //         optionsBuilder.UseSqlServer(connectionString);

       //        // return new AppDbContext(optionsBuilder.Options,_httpContextAccessor);
       //     }
       // }
        public IQueryable<T> ReadSet<T>() where T : class
         => Set<T>().AsNoTracking();

        protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);
            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
         
           
            
            //appointment configuration
            
          
          
 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }
        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecords> MedicalRecords { get; set; }
        public DbSet<Invoices> Invoices { get; set; }

        public DbSet<Payments> Payments { get; set; }
        public DbSet<Prescriptions> Prescriptions { get; set; }

        public DbSet<PrescriptionItems> PrescriptionItems { get; set; }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //var userId = _httpContextAccessor?.HttpContext?.User?
            //  .FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
            //foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.CreatedAt = DateTime.UtcNow;
            //            entry.Entity.CreatedBy = userId;
            //            break;
            //        case EntityState.Modified:
            //            entry.Entity.UpdatedAt = DateTime.UtcNow;
            //            entry.Entity.UpdatedBy = userId;
            //            break;
            //    }
            //}
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
{
    var userId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue("sub");
    foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
    {
        switch (entry.State)
        {
            case EntityState.Added:
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedBy = userId;
                break;
            case EntityState.Modified:
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedBy = userId;
                break;
        }
    }
}

    }
}
