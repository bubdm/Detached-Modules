using Detached.Modules.GraphQL.TypeExtensions;
using Detached.Modules.GraphQL.Validation;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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

        public static IRequestExecutorBuilder AddApplication(this IRequestExecutorBuilder builder, Application app)
        {
            foreach (Module module in app.Modules)
            {
                foreach (GraphQLComponent component in module.Components.OfType<GraphQLComponent>())
                {
                    builder.AddType(component.ImplementationType);
                }
            }

            builder.UseField<InputValidationMiddleware>();
            
            return builder;
        }
    }
}