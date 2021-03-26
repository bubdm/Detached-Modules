using Detached.Modules.GraphQL.Components;
using Detached.Modules.GraphQL.Filters;
using Detached.Modules.GraphQL.Validation;
using HotChocolate;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.GraphQL
{
    public static class Package
    {
        public static void AddGraphQL(this Module module, Action<IRequestExecutorBuilder> build)
        {
            module.Components.Add(new GraphQLComponent(build));
        }

        public static void AddMutation<TMutation>(this Module module)
        {
            module.Components.Add(new MutationComponent(typeof(TMutation)));
        }

        public static void AddQuery<TQuery>(this Module module)
        {
            module.Components.Add(new QueryComponent(typeof(TQuery)));
        }

        public static IRequestExecutorBuilder AddModule(this IRequestExecutorBuilder builder, Module module)
        {
            foreach (IComponent component in module.GetComponents())
            {
                switch (component)
                {
                    case MutationComponent mutation:
                        builder.AddType(mutation.GraphQLType);
                        break;
                    case QueryComponent query:
                        builder.AddType(query.GraphQLType);
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