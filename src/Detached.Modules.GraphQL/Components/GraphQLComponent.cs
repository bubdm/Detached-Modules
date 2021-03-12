using Detached.Modules.GraphQL.Filters;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.GraphQL
{
    public class GraphQLComponent : Component
    {
        public Type ComponentType { get; set; }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddErrorFilter<GraphQLErrorFilter>();

            services.AddScoped<IErrorHandler, X>();
        }

        class X : IErrorHandler
        {
            public IErrorBuilder CreateUnexpectedError(Exception exception)
            {
                throw new NotImplementedException();
            }

            public IError Handle(IError error)
            {
                throw new NotImplementedException();
            }
        }
    }
}