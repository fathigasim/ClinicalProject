

using ClinicProjectApplication.Common.Exceptions;
using ClinicProjectDomain.Entities;
using ClinicProjectDomain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace ClinicProjectApplication.Auth.Commands.RefreshTokens
{




    public class RefreshTokenHandler(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        ITokenService tokenService,
        TokenIssuer tokenIssuer)
        : IRequestHandler<RefreshTokenCommand, AuthTokenPair>
    {
        public async Task<AuthTokenPair> Handle(RefreshTokenCommand req, CancellationToken ct)
        {
            var user = await userRepository.GetByRefreshTokenAsync(req.RefreshToken, ct)
                ?? throw new UnauthorizedException("Invalid refresh token.");

            var existing = user.RefreshTokens.SingleOrDefault(t => t.Token == req.RefreshToken)
                ?? throw new UnauthorizedException("Invalid refresh token.");

            if (!existing.IsActive)
            {
                // Revoked token reuse = possible theft — nuke everything
                foreach (var t in user.RefreshTokens.Where(t => t.IsActive).ToList())
                    t.Revoke(req.IpAddress);
                await userRepository.SaveAsync(ct);
                throw new UnauthorizedException("Refresh token reuse detected. All sessions revoked.");
            }

            // Rotate: revoke the specific token, issue fresh pair
            existing.Revoke(req.IpAddress);
            return await tokenIssuer.IssueAsync(user, req.IpAddress, ct);
        }
        //public async Task<AuthTokenPair> Handle(RefreshTokenCommand req, CancellationToken ct)
        //{
        //    var principal = tokenService.GetPrincipalFromExpiredToken(req.AccessToken)
        //        ?? throw new UnauthorizedException("Invalid access token.");

        //    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
        //        ?? throw new UnauthorizedException("Invalid token claims.");

        //    var user = await userRepository.GetByRefreshTokenAsync(req.RefreshToken, ct)
        //        ?? throw new UnauthorizedException("Invalid refresh token.");

        //    if (user.Id != userId)
        //        throw new UnauthorizedException("Token mismatch.");

        //    var existing = user.RefreshTokens.SingleOrDefault(t => t.Token == req.RefreshToken)
        //        ?? throw new UnauthorizedException("Invalid refresh token.");
        //    if (!existing.IsActive)
        //    {
        //        // Revoked token reuse = possible theft — nuke everything
        //        foreach (var t in user.RefreshTokens.Where(t => t.IsActive).ToList())
        //            t.Revoke(req.IpAddress);
        //        await userRepository.SaveAsync(ct);
        //        throw new UnauthorizedException("Refresh token reuse detected. All sessions revoked.");
        //    }

        //    if (!existing.IsActive)
        //        throw new UnauthorizedException("Refresh token is expired or revoked.");
        //    // Rotate: revoke the specific token, issue fresh pair
        //    existing.Revoke(req.IpAddress);

        //    return await tokenIssuer.IssueAsync(user, req.IpAddress, ct);
        //}
    }
}