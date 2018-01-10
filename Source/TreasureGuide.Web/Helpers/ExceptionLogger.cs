using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TreasureGuide.Web.Helpers
{
    public class ExceptionLoggerAttribute : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var factory = serviceProvider.GetService<ILoggerFactory>();
            return new ExceptionLoggerFilter(factory);
        }

        public bool IsReusable { get; } = true;
    }

    public class ExceptionLoggerFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionLoggerFilter(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<ExceptionLoggerFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            var eventId = new EventId();
            _logger.LogCritical(eventId, context?.Exception, context?.Exception?.Message);
        }
    }
}
