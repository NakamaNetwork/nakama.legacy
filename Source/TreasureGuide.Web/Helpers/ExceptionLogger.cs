using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TreasureGuide.Common.Helpers
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

    public class ExceptionLoggerFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ExceptionLoggerFilter(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<ExceptionLoggerFilter>();
        }

        public override void OnException(ExceptionContext context)
        {
            var exceptionMessage = LogException(context.Exception);
            var pathMessage = LogRequest(context.HttpContext.Request);
            _logger.LogError(String.Join(" : ", pathMessage, exceptionMessage));
        }

        private string LogRequest(HttpRequest request)
        {
            return $"[{request.Method}]{request.Path}{request.QueryString}";
        }

        private string LogException(Exception contextException)
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
            return contextException.Message;
        }
    }
}
