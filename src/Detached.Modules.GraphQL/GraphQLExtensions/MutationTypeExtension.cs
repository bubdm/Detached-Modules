using HotChocolate.Types;

namespace QuickApi.Services
{
    public class MutationTypeExtension<T> : ObjectTypeExtension<T>
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            descriptor.Name("Mutation");
        }
    }
}