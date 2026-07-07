using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    // Domain/Entities/ApplicationUser.cs
    public class ApplicationUser : IdentityUser
    {
        private readonly List<RefreshToken> _refreshTokens = [];
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        public void AddRefreshToken(RefreshToken token) => _refreshTokens.Add(token);
        public void RemoveRefreshToken(RefreshToken token) => _refreshTokens.Remove(token);
    }


}
