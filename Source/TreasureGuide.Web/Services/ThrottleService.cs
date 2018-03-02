using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Helpers;

namespace TreasureGuide.Web.Services
{
    public interface IThrottleService
    {
        bool CanAccess(ClaimsPrincipal user, HttpRequest request, string path = null, double? expirationSeconds = null, int? limitation = 1, string extra = null);
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

        public bool CanAccess(ClaimsPrincipal user, HttpRequest request, string path = null, double? expirationSeconds = null, int? limitation = 1, string extra = null)
        {
            if (user?.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator) ?? false)
            {
                return true;
            }
            var key = GenerateKey(request, path, extra);
            var count = 0;
            var cached = _cache.TryGetValue(key, out count);
            var timeout = GenerateTimeout(expirationSeconds);
            if (!cached)
            {
                _cache.Set(key, count, timeout);
            }
            else
            {
                _cache.Set(key, ++count, timeout);
            }
            return count < limitation;
        }

        private string GenerateKey(HttpRequest request, string path = null, string extra = null)
        {
            var connection = request.HttpContext.Connection;
            var extraKey = "Unknown";
            StringValues agentKey;
            if (request.Headers.TryGetValue("User-Agent", out agentKey))
            {
                extraKey = String.Join(":", agentKey);
            }
            var items = new List<string>
            {
                connection.RemoteIpAddress?.ToString(), connection.RemotePort.ToString(), extraKey
            };
                items.Add(path);
                items.Add(extra);
            return String.Join("||", items);
        }

        private DateTimeOffset GenerateTimeout(double? seconds = null)
        {
            return DateTimeOffset.Now.AddSeconds(seconds ?? Seconds);
        }
    }
}
