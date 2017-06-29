using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TreasureGuide.Web.Helpers
{
    public static class HttpHelper
    {
        public static string CreateQuerystring(IEnumerable<KeyValuePair<string, string>> collection, string route = "")
        {
            var queryString = String.Join("&", collection.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value)}"));
            var result = String.Join(route.Contains("?") ? "&" : "?", route, queryString);
            return result;
        }
    }
}
