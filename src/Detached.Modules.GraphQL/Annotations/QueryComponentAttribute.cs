using Detached.Modules.GraphQL.Components;
using System;

namespace Detached.Modules.GraphQL.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class QueryComponentAttribute : Attribute, IComponentType
    {
        public void AddToModule(Type type, Module module)
        {
            module.Components.Add(new QueryComponent(type));
        }
    }
}