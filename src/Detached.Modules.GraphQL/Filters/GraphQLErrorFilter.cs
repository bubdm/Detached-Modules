using HotChocolate;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;

namespace Detached.Modules.GraphQL.Filters
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        readonly static JsonSerializerOptions _errorJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        readonly ILogger _logger;

        public GraphQLErrorFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("GraphQLUnmanaged");
        }

        public IError OnError(IError error)
        {
            string message = null;
            string key = null;
            string messageTemplate = null;
            Dictionary<string, object> arguments = null;
            string field = null;
            string debugMessage = null;

            if (error.Exception != null)
            {
                if (error.Exception is ErrorException errorException)
                {
                    message = errorException.Message;
                    key = errorException.Key;
                    messageTemplate = errorException.MessageTemplate;
                    arguments = errorException.Arguments;
                    field = errorException.Field;
                    debugMessage = errorException.DebugMessage;
                }
                else
                {
                    message = "An unexpected error has ocurred.";
                    key = "InternalServerError";
                    debugMessage = error.Exception.ToString();
                }

                error = error
                    .WithMessage(message)
                    .WithExtensions(new Dictionary<string, object>
                    {
                        { nameof(key), key },
                        { nameof(messageTemplate), messageTemplate },
                        { nameof(arguments), arguments },
                        { nameof(debugMessage), debugMessage },
                        { nameof(field), field }
                    });
            }

             _logger.LogError(error.Exception, "Request failed");

            return error;
        }
    }
}