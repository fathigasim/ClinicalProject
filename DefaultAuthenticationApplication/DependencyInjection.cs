
using ClinicProjectApplication.Auth.Commands.RegisterUser;
using ClinicProjectApplication.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ClinicProjectApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
this IServiceCollection services)
        {


            var assembly = typeof(DependencyInjection).Assembly;

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assembly);
            }, assembly);
            // MediatR pipeline (order is significant)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));
            services.AddValidatorsFromAssembly(assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
          
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            return services;
        }
        }
}
