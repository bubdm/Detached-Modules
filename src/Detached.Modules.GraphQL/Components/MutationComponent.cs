using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Detached.Modules.GraphQL.Components
{
    public class MutationComponent : IComponent
    {
        public static string MutationTypeName = "Mutation";

        public Type ImplementationType { get; set; }

        public void ConfigureServices(IModule module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }
    }
}