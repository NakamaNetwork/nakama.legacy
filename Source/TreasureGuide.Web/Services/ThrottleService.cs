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
        bool CanAccess(ClaimsPrincipal user, HttpRequest request, RouteData routeData = null, double? seconds = null);
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

        public bool CanAccess(ClaimsPrincipal user, HttpRequest request, RouteData routeData = null, double? seconds = null)
        {
            if (user.IsInAnyRole(RoleConstants.Administrator, RoleConstants.Moderator))
            {
                return true;
            }
            var now = DateTimeOffset.Now;
            var key = GenerateKey(request, routeData);
            DateTimeOffset? timestamp = null;
            var cached = _cache.TryGetValue(key, out timestamp);
            if (!cached)
            {
                var timeout = GenerateTimeout(seconds);
                _cache.Set(key, now, timeout);
            }
            return !cached;
        }

        private string GenerateKey(HttpRequest request, RouteData routeData = null)
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
            if (routeData != null)
            {
                items.Add(String.Join("/", routeData.Values["controller"], routeData.Values["action"]));
            }
            return String.Join("||", items);
        }

        private DateTimeOffset GenerateTimeout(double? seconds = null)
        {
            return DateTimeOffset.Now.AddSeconds(seconds ?? Seconds);
        }
    }
}
