
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken ct);
        Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct);
        Task<ApplicationUser?> GetByRefreshTokenAsync(string token, CancellationToken ct);
        Task UpdateAsync(ApplicationUser user, CancellationToken ct);
        Task PurgeExpiredTokensAsync(CancellationToken ct);
    }
}
