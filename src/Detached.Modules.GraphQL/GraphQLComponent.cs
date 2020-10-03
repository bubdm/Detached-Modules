using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.GraphQL
{
    public class GraphQLComponent : DetachedComponent
    {
        public Type ComponentType { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
      
        }
    }
}