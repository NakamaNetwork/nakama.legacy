using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Common.Helpers
{
    public class ThrottlingAttribute : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var service = serviceProvider.GetService<IThrottleService>();
            return new ThrottlingFilter(factory, service);
        }

        public bool IsReusable { get; } = true;
    }

    public class ThrottlingFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly ILogger _logger;
        private readonly IThrottleService _service;

        public ThrottlingFilter(ILoggerFactory factory, IThrottleService service)
        {
            _service = service;
            _logger = factory.CreateLogger<ExceptionLoggerFilter>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                if (!_service.CanAccess(null, controller.Request, null, 0.5, 25, "filter"))
                {
                    var userString = controller.User?.GetId();
                    _logger.LogWarning($"User {(!String.IsNullOrWhiteSpace(userString) ? $"'{userString}'" : "")}" +
                            $" from '{context.HttpContext.Connection.RemoteIpAddress}' was throttled when" +
                            $"trying to access '{controller.Request.Path}'");
                    context.Result = new StatusCodeResult(429);
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
