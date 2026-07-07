
using ClinicProjectDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ClinicProjectApplication.Interfaces
{
    public interface ITokenService1
    {
        string GenerateAccessToken(ApplicationUser user, IList<string> roles);

        RefreshToken GenerateRefreshToken(string userId, string? ipAddress = null);

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
