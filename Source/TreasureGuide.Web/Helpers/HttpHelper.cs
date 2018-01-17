using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public static string CreateQuerystring(string route = "", ModelStateDictionary modelState = null)
        {
            return CreateQuerystring(route, new KeyValuePair<string, string>("message", String.Join(": ", "Some errors occurred:", modelState.ConcatErrors())));
        }

        public static string ConcatErrors(this ModelStateDictionary modelState)
        {
            var all = modelState.Values.SelectMany(v => v.Errors.Select(b => String.IsNullOrWhiteSpace(b.ErrorMessage)
            ? b.Exception.Message.Split(new[] { "Path" }, StringSplitOptions.None).FirstOrDefault() ?? ""
            : b.ErrorMessage));
            return String.Join(", ", all);
        }
    }
}
