

using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication
{
    public class TokenIssuer(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IUserRepository userRepository)
    {
        public async Task<AuthTokenPair> IssueAsync(
            ApplicationUser user, string? ipAddress, CancellationToken ct)
        {
            // 1. Revoke currently active tokens (persists audit data)
            foreach (var t in user.RefreshTokens.Where(t => t.IsActive).ToList())
                t.Revoke(ipAddress);

            // 2. Prune previously inactive tokens
            foreach (var t in user.RefreshTokens.Where(t => !t.IsActive).ToList())
                user.RemoveRefreshToken(t);

            // 3. Issue new pair
            var newRefresh = tokenService.GenerateRefreshToken(user.Id, ipAddress);
            user.AddRefreshToken(newRefresh);

            var roles = await userManager.GetRolesAsync(user);
            var accessToken = tokenService.GenerateAccessToken(user, roles);

            await userRepository.UpdateAsync(user, ct);

            return new AuthTokenPair(
                accessToken,
                newRefresh.Token,
                newRefresh.Expires,
                DateTime.UtcNow.AddMinutes(15));
        }
    }
}
