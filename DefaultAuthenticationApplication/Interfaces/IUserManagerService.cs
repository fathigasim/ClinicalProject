
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IUserManagerService
    {
        Task<ApplicationUser?> FindByEmailAsync(string Email,CancellationToken cancellationToken);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task DeleteUserAsync(ApplicationUser user);
    }
}
