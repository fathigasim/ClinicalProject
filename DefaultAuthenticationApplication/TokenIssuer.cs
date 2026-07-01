

using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication
{
    //public class TokenIssuer(
    //UserManager<ApplicationUser> userManager,
    //ITokenService tokenService,
    //IUserRepository userRepository,
    //IConfiguration configuration
    //)
    //{
    //    public async Task<AuthTokenPair> IssueAsync(
    // ApplicationUser user, string? ipAddress, CancellationToken ct)
    //    {
    //        foreach (var t in user.RefreshTokens.Where(t => t.IsActive).ToList())
    //            t.Revoke(ipAddress);

    //        foreach (var t in user.RefreshTokens.Where(t => !t.IsActive).ToList())
    //            user.RemoveRefreshToken(t);

    //        var newRefresh = tokenService.GenerateRefreshToken(user.Id, ipAddress);
    //        user.AddRefreshToken(newRefresh);

    //        var roles = await userManager.GetRolesAsync(user);
    //        var accessToken = tokenService.GenerateAccessToken(user, roles);

    //        await userRepository.SaveAsync(ct); // just save, no Update() ever

    //        return new AuthTokenPair(accessToken, newRefresh.Token,
    //            newRefresh.Expires, DateTime.UtcNow.AddMinutes(double.Parse( configuration["JwtSettings:AccessTokenExpiryMinutes"])));
    //    }
    //}
    public class TokenIssuer(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IUserRepository userRepository,
    IConfiguration configuration
    )
    {
        public async Task<AuthTokenPair> IssueAsync(
            ApplicationUser user, string? ipAddress, CancellationToken ct, string amr = "pwd")
        {
            foreach (var t in user.RefreshTokens.Where(t => t.IsActive).ToList())
                t.Revoke(ipAddress);
            foreach (var t in user.RefreshTokens.Where(t => !t.IsActive).ToList())
                user.RemoveRefreshToken(t);

            var newRefresh = tokenService.GenerateRefreshToken(user.Id, ipAddress);
            user.AddRefreshToken(newRefresh);

            var roles = await userManager.GetRolesAsync(user);
            var accessToken = tokenService.GenerateAccessToken(user, roles, amr);

            await userRepository.SaveAsync(ct); // just save, no Update() ever

            return new AuthTokenPair(
     accessToken,
     newRefresh.Token,
     newRefresh.Expires,
     DateTime.UtcNow.AddMinutes(double.Parse(configuration["JwtSettings:AccessTokenExpiryMinutes"])));
        }
    }
}
