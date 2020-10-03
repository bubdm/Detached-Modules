using HotChocolate.Types;

namespace QuickApi.Services
{
    public class QueryTypeExtension<T> : ObjectTypeExtension<T>
    {
        protected override void Configure(IObjectTypeDescriptor<T> descriptor)
        {
            descriptor.Name("Query");
        }
    }
}
