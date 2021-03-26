using Detached.Modules.GraphQL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Detached.Modules.GraphQL.Components
{
    public class MutationComponent : IComponent
    {
        public static string MutationTypeName = "Mutation";

        public MutationComponent(Type implementationType)
        {
            ImplementationType = implementationType;
            GraphQLType = typeof(MutationTypeExtension<>).MakeGenericType(implementationType);
        }

        public Type GraphQLType { get; }

        public Type ImplementationType { get; }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo
            (
                ImplementationType.Name,
                "Mutation (GraphQL)",
                null
            );
        }
    }
}