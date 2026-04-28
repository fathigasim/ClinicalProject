using ClinicProjectApplication.Interfaces;
using ClinicProjectDomain.Entities;
using ClinicProjectInfrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultAuthenticationInfrastructure.Services
{
    public class UserManagerService :IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        public UserManagerService(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _dbContext.users
    
    .FirstOrDefaultAsync(u => u.Email == email, ct);
            return user;
        }

        public async Task< bool> CheckPasswordAsync(ApplicationUser user,string password)
        {
           
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        
    }
}
