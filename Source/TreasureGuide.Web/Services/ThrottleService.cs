using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace TreasureGuide.Web.Services
{
    public interface IThrottleService
    {
        bool CanAccess(HttpRequest request);
    }

    public class ThrottleService : IThrottleService
    {
        private readonly IMemoryCache _cache;

        public ThrottleService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool CanAccess(HttpRequest request)
        {
            var now = DateTimeOffset.Now;
            var key = GenerateKey(request);
            DateTimeOffset? timestamp = null;
            var cached = _cache.TryGetValue(key, out timestamp);
            if (cached)
            {
                _cache.Remove(key);
            }
            var timeout = GenerateTimeout(request);
            _cache.Set(key, now, timeout);
            return !cached;
        }

        private string GenerateKey(HttpRequest request)
        {
            var connection = request.HttpContext.Connection;
            var extraKey = "Unknown";
            StringValues agentKey;
            if (request.Headers.TryGetValue("User-Agent", out agentKey))
            {
                extraKey = String.Join(":", agentKey);
            }
            return String.Join("||", connection.RemoteIpAddress, connection.RemotePort, extraKey);
        }

        private DateTimeOffset GenerateTimeout(HttpRequest request)
        {
            return DateTimeOffset.Now.AddSeconds(30);
        }
    }
}
