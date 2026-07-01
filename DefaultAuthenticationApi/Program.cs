using ClinicProjectApi;
using ClinicProjectApplication;
using ClinicProjectInfrastructure.Persistence;
using DefaultAuthenticationApi.Middleware;
using DefaultAuthenticationApplication;
using DefaultAuthenticationInfrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using OtpNet;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication(builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
//    });
// CORRECT — both policies registered at the top level
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("AdminUser", policy =>
       policy.RequireRole("Admin","User"));
});

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");
logger.LogInformation($"AllowedOrigins: {string.Join(",", builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())}");
Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
Console.WriteLine($"AllowedOrigins: {string.Join(",", builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())}");
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

app.MapControllers();

app.Run();
