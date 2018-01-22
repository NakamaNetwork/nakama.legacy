using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

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

    public class RedirectWwwRule : IRule
    {
        public int StatusCode { get; } = (int)HttpStatusCode.MovedPermanently;
        public bool ExcludeLocalhost { get; set; } = true;

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var host = request.Host;
            if (host.Host.StartsWith("www", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            if (ExcludeLocalhost && string.Equals(host.Host, "localhost", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var newPath = request.Scheme + "://www." + host.Value + request.PathBase + request.Path + request.QueryString;

            var response = context.HttpContext.Response;
            response.StatusCode = StatusCode;
            response.Headers[HeaderNames.Location] = newPath;
            context.Result = RuleResult.EndResponse;
        }
    }
}
