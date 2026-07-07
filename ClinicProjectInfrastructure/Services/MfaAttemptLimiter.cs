using ClinicProjectApplication.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectInfrastructure.Services
{
    public class MfaAttemptLimiter : IMfaAttemptLimiter
    {
        private readonly IMemoryCache _cache;
        private const int MaxAttempts = 5;
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(5);

        public MfaAttemptLimiter(IMemoryCache cache) => _cache = cache;

        public bool IsAllowed(string mfaToken)
        {
            var count = _cache.Get<int?>(CacheKey(mfaToken)) ?? 0;
            return count < MaxAttempts;
        }

        public void RecordFailure(string mfaToken)
        {
            var key = CacheKey(mfaToken);
            var count = (_cache.Get<int?>(key) ?? 0) + 1;
            _cache.Set(key, count, Window);
        }

        private static string CacheKey(string mfaToken) => $"mfa_attempts:{mfaToken}";
    }

}
