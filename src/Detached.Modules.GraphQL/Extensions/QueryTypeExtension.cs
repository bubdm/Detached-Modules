using Detached.Modules.GraphQL.Components;
using HotChocolate.Types;

namespace Detached.Modules.GraphQL.Extensions
{
    public class QueryTypeExtension<TQuery> : ObjectTypeExtension<TQuery>
    {
        protected override void Configure(IObjectTypeDescriptor<TQuery> descriptor)
        {
            descriptor.Name(QueryComponent.QueryTypeName);
        }
    }
}
