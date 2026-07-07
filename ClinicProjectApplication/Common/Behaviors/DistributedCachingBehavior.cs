using ClinicProjectApplication.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Behaviors
{
    public class DistributedCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : ICacheableQuery
    {
        private readonly IDistributedCache _cache;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var cached = await _cache.GetStringAsync(request.CacheKey, ct);
            if (cached != null)
                return JsonSerializer.Deserialize<TResponse>(cached);

            var response = await next();

            await _cache.SetStringAsync(request.CacheKey, JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = request.Expiration ?? TimeSpan.FromMinutes(5)
                }, ct);

            return response;
        }
    }
}
