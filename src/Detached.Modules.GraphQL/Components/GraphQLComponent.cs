using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Detached.Modules.GraphQL.Components
{
    public class GraphQLComponent : IComponent
    {
        public GraphQLComponent(Action<IRequestExecutorBuilder> build)
        {
            Build = build;
        }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo
            (
                "Inline Configuration",
                "Configuration (GraphQL)",
                null
            );
        }

        public Action<IRequestExecutorBuilder> Build { get; }
    }
}
