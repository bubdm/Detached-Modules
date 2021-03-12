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
        public static void AddMutation<TMutation>(this ComponentCollection components)
        {
            components.Add(new GraphQLComponent { ComponentType = typeof(MutationTypeExtension<TMutation>) });
        }

        public static void AddQuery<TQuery>(this ComponentCollection components)
        {
            components.Add(new GraphQLComponent { ComponentType = typeof(QueryTypeExtension<TQuery>) });
        }

        public static IRequestExecutorBuilder AddApplication(this IRequestExecutorBuilder builder, Application app)
        {
            foreach (Module module in app.Modules)
            {
                foreach (GraphQLComponent component in module.Components.OfType<GraphQLComponent>())
                {
                    builder.AddType(component.ComponentType);
                }
            }

            builder.UseField<InputValidationMiddleware>();
            

            return builder;
        }
    }
}