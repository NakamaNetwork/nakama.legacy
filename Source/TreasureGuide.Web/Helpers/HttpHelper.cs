using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace TreasureGuide.Web.Helpers
{
    public static class HttpHelper
    {
        public static string CreateQuerystring(string route = "", params KeyValuePair<string, string>[] collection)
        {
            var queryString = String.Join("&", collection.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
            var result = String.Join(route.Contains("?") ? "&" : "?", route, queryString);
            return result;
        }

        public static string GetAccessToken(this ExternalLoginInfo info)
        {
            return info.AuthenticationTokens.SingleOrDefault(x => x.Name == "access_token")?.Value;
        }
    }
}
