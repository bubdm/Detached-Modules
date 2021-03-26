using Detached.Modules.GraphQL.Annotations;
using HotChocolate;

namespace Detached.Modules.GraphQLSample.Modules.System.Services
{
    [QueryComponent]
    public class SystemQuery
    {
        public ModuleInfo SystemGetModules([Service] Module root)
        {
            return root.GetInfo();
        }
    }
}