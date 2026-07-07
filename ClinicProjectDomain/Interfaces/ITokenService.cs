using ClinicProjectDomain.Entities;

using System.Security.Claims;


namespace ClinicProjectDomain.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles,string amr);
        RefreshToken GenerateRefreshToken(string userId, string? ip);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
