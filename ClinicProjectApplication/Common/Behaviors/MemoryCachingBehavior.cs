using ClinicProjectApplication.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Behaviors
{

    public class MemoryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheableQuery
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCachingBehavior<TRequest, TResponse>> _logger;

        public MemoryCachingBehavior(IMemoryCache cache, ILogger<MemoryCachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            // 1. Check if the result is already in cache
            if (_cache.TryGetValue(request.CacheKey, out TResponse cachedResponse))
            {
                _logger.LogInformation("Cache hit for {CacheKey}", request.CacheKey);
                return cachedResponse;
            }

            // 2. If not, execute the actual handler (the "next" delegate)
            _logger.LogInformation("Cache miss for {CacheKey}. Fetching from source.", request.CacheKey);
            var response = await next();

            // 3. Store the result in cache for next time
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = request.Expiration ?? TimeSpan.FromMinutes(5)
            };

            _cache.Set(request.CacheKey, response, cacheOptions);

            return response;
        }
    }

}
