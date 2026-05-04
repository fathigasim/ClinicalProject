

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Text;
using ClinicProjectInfrastructure.Persistence;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectInfrastructure.Services;
using ClinicProjectDomain.Interfaces;
using DefaultAuthenticationInfrastructure.Services;
using ClinicProjectInfrastructure.Persistence.Repositories;
using ClinicProjectInfrastructure.Identity;
using ClinicProjectApplication;
using ClinicProjectDomain.Services;




namespace DefaultAuthenticationInfrastructure
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddInfrastructure(
          
    this IServiceCollection services,
     
    IConfiguration configuration)
        {
          
            // ── Single SQL Server connection for both reads and writes ─────────────
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql =>
                    {
                        
                        sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    }));

            // ── Same scoped AppDbContext satisfies both IUnitOfWork and IReadDbContext
            services.AddScoped<IUnitOfWork, UnitOfWork>();
          //  services.AddScoped<IReadDbContext>(sp => sp.GetRequiredService<AppDbContext>());

            // ── Identity ──────────────────────────────────────────────────────────
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.User.RequireUniqueEmail = true;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opts.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // ── JWT Authentication ────────────────────────────────────────────────
            //var jwtSettings = configuration
            //    .GetSection("JwtSettings");


            //services.Configure<JwtSettings>(jwtSettings);
            // Add JWT authentication
    //        var jwt = configuration
    //.GetSection(JwtSettings.Section)
    //.Get<JwtSettings>()!;
            
            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,  // no tolerance — important!
                    };

                    opts.Events = new JwtBearerEvents
                    {
                        OnChallenge = ctx =>
                        {
                            ctx.HandleResponse();
                            ctx.Response.StatusCode = 401;
                            ctx.Response.ContentType = "application/problem+json";
                            return ctx.Response.WriteAsync(
                                """{"title":"Unauthorized","status":401}""");
                        },
                        OnForbidden = ctx =>
                        {
                            ctx.Response.StatusCode = 403;
                            ctx.Response.ContentType = "application/problem+json";
                            return ctx.Response.WriteAsync(
                                """{"title":"Forbidden","status":403}""");
                        },
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });
            // ── Repositories & services ───────────────────────────────────────────
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository,AppointmentRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();    
            services.AddScoped<IWeeklyScheduleRepository, WeeklyScheduleRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<TokenIssuer>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IReadDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddScoped<ISequenceService,SequenceService>();
            services.AddTransient<ScheduleService>();
            //services.AddScoped<IDbSeeder,DbSeeder>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
    }

