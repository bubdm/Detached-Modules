using HotChocolate;
using System.Collections.Generic;

namespace Detached.Modules.GraphQL.Filters
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            string message = null;
            string key = null;
            string messageTemplate = null;
            Dictionary<string, object> arguments = null;
            string debugMessage = null;

            if (error.Exception != null)
            {
                if (error.Exception is ErrorException errorException)
                {
                    message = errorException.Message;
                    key = errorException.Key;
                    messageTemplate = errorException.MessageTemplate;
                    arguments = errorException.Arguments;
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
                        { nameof(debugMessage), debugMessage }
                    });
            }

            return error;
        }
    }
}