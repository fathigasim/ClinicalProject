
using ClinicProjectApplication.Auth.Commands.RegisterUser;
using ClinicProjectApplication.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ClinicProjectApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
this IServiceCollection services, IHostEnvironment env)
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
            // swap cache behavior based on environment
            if (env.IsDevelopment())
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCachingBehavior<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCacheInvalidationBehavior<,>));
            }
            else
            {
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DistributedCachingBehavior<,>));
                services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DistributedCacheInvalidationBehavior<,>));
            }
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            return services;
        }
        }
}
