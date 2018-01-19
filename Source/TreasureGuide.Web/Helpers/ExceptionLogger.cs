using System;
using System.Data.Entity.Validation;
using System.Linq;
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
            LogException(context.Exception);
        }

        private void LogException(Exception contextException)
        {
            var validation = contextException as DbEntityValidationException;
            if (validation != null)
            {
                var errorMessages = validation.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = String.Concat(validation.Message, " The validation errors are: ", fullErrorMessage);
                contextException = new DbEntityValidationException(exceptionMessage, validation.EntityValidationErrors);
            }
            _logger.LogError(contextException.ToString());
        }
    }
}
