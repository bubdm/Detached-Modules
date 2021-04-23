using Detached.Modules.GraphQL.Annotations;
using HotChocolate;
using System;

namespace Detached.Modules.GraphQLSample.Modules.System.Services
{
    [QueryComponent]
    public class SystemQuery
    {
        public ModuleInfo SystemGetModules([Service] Module root)
        {
            return root.GetInfo();
        }

        public bool SystemThrowException()
        {
            throw new ApplicationException("this is a sample exception");
        }
    }
}