using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Helpers;

namespace TreasureGuide.Web.Services
{
    public interface IThrottleService
    {
        bool CanAccess(ClaimsPrincipal user, HttpRequest request);
    }

    public class ThrottleService : IThrottleService
    {
        public const double Seconds = 15;
        public const string Message = "Please wait a few seconds before submitting again.";
        private readonly IMemoryCache _cache;

        public ThrottleService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool CanAccess(ClaimsPrincipal user, HttpRequest request)
        {
            if (user.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                return true;
            }
            var now = DateTimeOffset.Now;
            var key = GenerateKey(request);
            DateTimeOffset? timestamp = null;
            var cached = _cache.TryGetValue(key, out timestamp);
            if (!cached)
            {
                var timeout = GenerateTimeout(request);
                _cache.Set(key, now, timeout);
            }
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
            return DateTimeOffset.Now.AddSeconds(Seconds);
        }
    }
}
