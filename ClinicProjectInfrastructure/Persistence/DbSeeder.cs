
using ClinicProjectDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Persistence
{
    public static class DbSeeder 
    {
        private static readonly string[] Roles = ["Admin", "User"];

        public static async Task SeedAsync(this IHost host)
        {
            await using var scope = host.Services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            await db.Database.MigrateAsync();

            // Seed roles
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Role '{Role}' created.", role);
                }
            }

            // Seed default admin (only in Development — driven by env in Program.cs)
            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin@123";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                   
                    Email = adminEmail,
                    UserName = adminEmail,
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    logger.LogInformation("Default admin account created.");
                }
                else
                {
                    logger.LogWarning("Failed to create admin: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

        }
    }
}