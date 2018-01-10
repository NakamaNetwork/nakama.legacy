using System;
using System.Data.Entity.Validation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            HandleException(context.Exception);
        }

        private void HandleException(Exception exception)
        {
            Serialize(exception);
        }

        private void Serialize(Exception exception)
        {
            var full = JsonConvert.SerializeObject(exception);
            _logger.LogError(full);
        }
    }
}
