using Detached.Modules.GraphQL.TypeExtensions;
using Detached.Modules.GraphQL.Validation;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules.GraphQL
{
    public static class Package
    {
        public static void AddMutation<TMutation>(this IModule module)
        {
            module.Components.Add(new GraphQLComponent { ImplementationType = typeof(MutationTypeExtension<TMutation>) });
        }

        public static void AddQuery<TQuery>(this IModule module)
        {
            module.Components.Add(new GraphQLComponent { ImplementationType = typeof(QueryTypeExtension<TQuery>) });
        }

        public static IRequestExecutorBuilder AddModule(this IRequestExecutorBuilder builder, IModule module)
        {
            foreach (IComponent component in module.GetAllComponents())
            {
                if (component is GraphQLComponent gql)
                {
                    builder.AddType(gql.ImplementationType);
                }
            }

            builder.UseField<InputValidationMiddleware>();
            
            return builder;
        }
    }
}