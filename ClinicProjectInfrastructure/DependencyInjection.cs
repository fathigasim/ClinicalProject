

using ClinicProjectApplication;
using ClinicProjectApplication.Common;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectDomain.Services;
using ClinicProjectInfrastructure.Identity;
using ClinicProjectInfrastructure.Persistence;
using ClinicProjectInfrastructure.Persistence.Repositories;
using ClinicProjectInfrastructure.Services;
using ClinicProjectInfrastructure.Services.ClinicProjectApplication.Auth.Services;
using DefaultAuthenticationInfrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Text;




namespace DefaultAuthenticationInfrastructure
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddInfrastructure(
          
    this IServiceCollection services,
     
    IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(
    configuration.GetSection("SmtpSettings"));

            // ── Single SQL Server connection for both reads and writes ─────────────
            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql =>
                    {
                        
                        sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    }));
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
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


                opts.Lockout.MaxFailedAccessAttempts = 5;
                opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                opts.Lockout.AllowedForNewUsers = true;
                opts.SignIn.RequireConfirmedEmail = true; // ties into point 1
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
                    opts.MapInboundClaims = false;   // ← add this
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
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(AppUserRoles.Admin));
                options.AddPolicy("UserOnly", policy => policy.RequireRole(AppUserRoles.Admin, AppUserRoles.User));
            });
            //Brevo Email Configurations Typed Client
            services.Configure<BrevoSettings>(configuration.GetSection("Brevo"));
            services.AddHttpClient<IEmailSender, BrevoEmailService>(client =>
            {
                client.BaseAddress = new Uri("https://api.brevo.com/v3/");
            }).AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());
            ;
           
            // ── Repositories & services ───────────────────────────────────────────
            services.AddMemoryCache();
            services.AddSingleton<IMfaAttemptLimiter, MfaAttemptLimiter>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository,AppointmentRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IWeeklyScheduleRepository, WeeklyScheduleRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IStripeService, StripeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<TokenIssuer>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IReadDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.AddScoped<ISequenceService,SequenceService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPaymentReportService, PaymentReportService>();
            services.AddSingleton<IMfaChallengeTokenService, MfaChallengeTokenService>();
            services.AddTransient<ScheduleService>();
            //services.AddTransient<IEmailSender, EmailSender>();
            services.AddHostedService<TokenCleanupService>();
            //services.AddScoped<IDbSeeder,DbSeeder>();
            services.AddHttpContextAccessor();

            return services;
        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
HttpPolicyExtensions
.HandleTransientHttpError() // 5xx, 408, network failures
.OrResult(msg => msg.StatusCode ==
HttpStatusCode.TooManyRequests)
.WaitAndRetryAsync(3, retryAttempt =>
TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); //
                                                  //      exponential backoff
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        =>
        HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
    }

