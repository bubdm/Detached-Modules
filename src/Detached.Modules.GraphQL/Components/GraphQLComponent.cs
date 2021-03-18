using Detached.Modules.GraphQL.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Detached.Modules.GraphQL
{
    public class GraphQLComponent : IComponent
    {
        public IModule Module { get; set; }

        public Type ImplementationType { get; set; }

        public void ConfigureServices(IModule module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddErrorFilter<GraphQLErrorFilter>();
        }
    }
}