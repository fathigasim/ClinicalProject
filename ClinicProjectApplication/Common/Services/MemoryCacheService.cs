using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        // Holds a CancellationTokenSource for each prefix
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixTokens = new();

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, string prefix, T value, TimeSpan expiration)
        {
            var cts = _prefixTokens.GetOrAdd(prefix, _ => new CancellationTokenSource());

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration)
                .AddExpirationToken(new CancellationChangeToken(cts.Token)); // Binds entry to the prefix token

            _cache.Set(key, value, options);
        }

        public void RemoveByPrefix(string prefix)
        {
            // Canceling the token instantly evicts all IMemoryCache entries bound to this prefix
            if (_prefixTokens.TryRemove(prefix, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }
        }
    }
   
}
