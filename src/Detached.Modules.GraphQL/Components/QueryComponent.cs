using Detached.Modules.GraphQL.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Detached.Modules.GraphQL.Components
{
    public class QueryComponent : IComponent
    {
        public QueryComponent(Type implementationType)
        {
            GraphQLType = typeof(QueryTypeExtension<>).MakeGenericType(implementationType);
            ImplementationType = implementationType;
        }

        public static string QueryTypeName = "Query";

        public Type GraphQLType { get; set; }

        public Type ImplementationType { get; }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo
            (
                ImplementationType.Name,
                "Query (GraphQL)",
                null
            );
        }
    }
}