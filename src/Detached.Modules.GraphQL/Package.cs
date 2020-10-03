using HotChocolate;
using QuickApi.Services;
using System.Linq;

namespace Detached.Modules.GraphQL
{
    public static class Package
    {
        public static void AddMutation<TMutation>(this DetachedComponentCollection components)
        {
            components.Add(new GraphQLComponent { ComponentType = typeof(MutationTypeExtension<TMutation>) });
        }

        public static void AddQuery<TQuery>(this DetachedComponentCollection components)
        {
            components.Add(new GraphQLComponent { ComponentType = typeof(QueryTypeExtension<TQuery>) });
        }

        public static ISchemaBuilder UseApplication(this ISchemaBuilder schemaBuilder, DetachedApplication app)
        {
            foreach (DetachedModule module in app.Modules)
            {
                foreach (GraphQLComponent component in module.Components.OfType<GraphQLComponent>())
                {
                    schemaBuilder.AddType(component.ComponentType);
                }
            }

            return schemaBuilder;
        }
    }
}