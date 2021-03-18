using Detached.Modules.GraphQL.Components;
using HotChocolate.Types;

namespace Detached.Modules.GraphQL.Extensions
{
    public class MutationTypeExtension<TMutation> : ObjectTypeExtension<TMutation>
    {
        protected override void Configure(IObjectTypeDescriptor<TMutation> descriptor)
        {
            descriptor.Name(MutationComponent.MutationTypeName);
        }
    }
}