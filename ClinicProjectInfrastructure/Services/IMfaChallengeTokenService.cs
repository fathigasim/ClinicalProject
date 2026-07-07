using ClinicProjectApplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace ClinicProjectApplication.Auth.Services
    {
        public class MfaChallengeTokenService : IMfaChallengeTokenService
        {
            private readonly SymmetricSecurityKey _key;
            private const string Purpose = "mfa_challenge";
            private const int ExpiryMinutes = 5;

            public MfaChallengeTokenService(IConfiguration config)
            {
                var secret = config["JwtSettings:MfaChallengeSecret"]
                    ?? throw new InvalidOperationException("JwtSettings:MfaChallengeSecret is not configured.");

                _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            }

            public string GenerateChallengeToken(string userId)
            {
                var claims = new[]
                {
                new Claim("sub", userId),
                new Claim("purpose", Purpose)
            };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(ExpiryMinutes),
                    signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            public string? ValidateAndGetUserId(string token)
            {
                // MapInboundClaims = false is critical: without it, "sub" gets silently
                // remapped to ClaimTypes.NameIdentifier during validation and
                // FindFirstValue("sub") below always returns null.
                var handler = new JwtSecurityTokenHandler
                {
                    MapInboundClaims = false
                };

                try
                {
                    var principal = handler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = _key,
                        ClockSkew = TimeSpan.Zero
                    }, out _);

                    if (principal.FindFirstValue("purpose") != Purpose)
                        return null;

                    var userId = principal.FindFirstValue("sub");
                    return string.IsNullOrEmpty(userId) ? null : userId;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}