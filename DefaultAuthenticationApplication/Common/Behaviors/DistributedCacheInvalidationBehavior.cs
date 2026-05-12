using ClinicProjectApplication.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Behaviors
{
    // Distributed cache invalidation (Production)
    public class DistributedCacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheInvalidatorCommand
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<DistributedCacheInvalidationBehavior<TRequest, TResponse>> _logger;

        public DistributedCacheInvalidationBehavior(IDistributedCache cache, ILogger<DistributedCacheInvalidationBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var response = await next();
            foreach (var key in request.CacheKeys)
            {
                await _cache.RemoveAsync(key, ct);
                _logger.LogInformation("Invalidated distributed cache for key: {CacheKey}", key);
            }
            return response;
        }
    }
}
