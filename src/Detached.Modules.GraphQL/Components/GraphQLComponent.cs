using Detached.Modules.GraphQL.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.GraphQL
{
    public class GraphQLComponent : IComponent
    {
        public IModule Module { get; set; }

        public Type ImplementationType { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddErrorFilter<GraphQLErrorFilter>();
        }
    }
}