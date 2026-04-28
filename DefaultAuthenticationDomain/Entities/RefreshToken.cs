using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectDomain.Entities
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public string UserId { get; private set; } = default!;
        public string Token { get; private set; } = default!;
        public DateTime Expires { get; private set; }
        public string? CreatedByIp { get; private set; }
        public DateTime? Revoked { get; private set; }
        public string? RevokedByIp { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked is null && !IsExpired;

        private RefreshToken() { }

        public static RefreshToken Create(string userId, string token,
            DateTime expires, string? ip) => new()
            {
                UserId = userId,
                Token = token,
                Expires = expires,
                CreatedByIp = ip,
            };

        public void Revoke(string? ip)
        {
            Revoked = DateTime.UtcNow;
            RevokedByIp = ip;
        }
    }
}
