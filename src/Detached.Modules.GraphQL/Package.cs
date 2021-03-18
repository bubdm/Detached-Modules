using Detached.Modules.GraphQL.Components;
using Detached.Modules.GraphQL.Filters;
using Detached.Modules.GraphQL.TypeExtensions;
using Detached.Modules.GraphQL.Validation;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.GraphQL
{
    public static class Package
    {
        public static void AddGraphQL(this IModule module, Action<IRequestExecutorBuilder> build)
        {
            module.Components.Add(new GraphQLComponent(build));
        }

        public static void AddMutation<TMutation>(this IModule module)
        {
            module.Components.Add(new MutationComponent { ImplementationType = typeof(MutationTypeExtension<TMutation>) });
        }

        public static void AddQuery<TQuery>(this IModule module)
        {
            module.Components.Add(new QueryComponent { ImplementationType = typeof(QueryTypeExtension<TQuery>) });
        }

        public static IRequestExecutorBuilder AddModule(this IRequestExecutorBuilder builder, IModule module)
        {
            foreach (IComponent component in module.GetAllComponents())
            {
                switch (component)
                {
                    case MutationComponent mutation:
                        builder.AddType(mutation.ImplementationType);
                        break;
                    case QueryComponent query:
                        builder.AddType(query.ImplementationType);
                        break;
                    case GraphQLComponent gql:
                        gql.Build?.Invoke(builder);
                        break;
                } 
            }

            builder.UseField<InputValidationMiddleware>();
            builder.Services.AddErrorFilter<GraphQLErrorFilter>();

            return builder;
        }
    }
}