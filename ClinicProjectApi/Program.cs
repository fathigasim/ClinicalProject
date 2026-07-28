using ClinicProjectApi;
using ClinicProjectApplication;
using ClinicProjectApplication.Auth.Commands.PurgeTokens;
using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using ClinicProjectInfrastructure.Persistence;
using ClinicProjectInfrastructure.Services;
using DefaultAuthenticationApi.Middleware;
using DefaultAuthenticationApplication;
using DefaultAuthenticationInfrastructure;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    Log.Information("Starting up");

   

    builder.Host.UseSerilog((context, services, loggerConfig) => loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

    // Add services to the container.
    builder.Services.AddApplication(builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        });
  

    builder.Services.AddCors(options =>
{
    var origins = builder.Configuration
         .GetSection("AllowedOrigins")
         .Get<string[]>()!;
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins(origins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
     

        policy.WithOrigins(origins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("auth", o =>
    {
        o.PermitLimit = 5;
        o.Window = TimeSpan.FromMinutes(1);
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token in the format: Bearer {your_token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("AdminUser", policy =>
       policy.RequireRole("Admin","User"));
});
    builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));
    builder.Services.AddHangfireServer();
    

    var app = builder.Build();
    app.UseHangfireDashboard();
    // Program.cs — register once, on startup
    RecurringJob.AddOrUpdate<IMediator>(
        "purge-expired-tokens",
        mediator => mediator.Send(new PurgeExpiredTokensCommand(), CancellationToken.None),
        Cron.Daily); // or Cron.Daily(3) for 3 AM UTC
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var entityType = ctx.Model.FindEntityType(typeof(Appointment));
    var nav = entityType?.FindNavigation(nameof(Appointment.Invoice));
    Console.WriteLine($"Invoice navigation found: {nav != null}");
    Console.WriteLine($"Property access mode: {nav?.GetPropertyAccessMode()}");
    Console.WriteLine($"Assembly: {typeof(AppDbContext).Assembly.Location}");
    Console.WriteLine($"Assembly last write: {System.IO.File.GetLastWriteTime(typeof(AppDbContext).Assembly.Location)}");
    var configTypes = typeof(AppDbContext).Assembly.GetTypes()
    .Where(t => typeof(IEntityTypeConfiguration<>).IsAssignableFrom(t) == false // interface check needs generic handling
             && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
    .Select(t => t.FullName);

    foreach (var t in configTypes) Console.WriteLine(t);
    //    logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");
    //logger.LogInformation($"AllowedOrigins: {string.Join(",", builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())}");
    //Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
    //Console.WriteLine($"AllowedOrigins: {string.Join(",", builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())}");
    //var sharedKey = Console.ReadLine()!;
    //var key = Base32Encoding.ToBytes(sharedKey); // the SharedKey from step 3
    //var totp = new Totp(key);
    //var code = totp.ComputeTotp(); // current 6-digit code, valid ~30s
    //Console.WriteLine(code);
    await app.SeedAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseCors("DevCors");
}
// One UseCors call, picks policy based on environment
app.UseCors(app.Environment.IsDevelopment() ? "DevelopmentPolicy" : "ProductionPolicy");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire"); // secure this in prod, see step 6
app.MapControllers();

app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}