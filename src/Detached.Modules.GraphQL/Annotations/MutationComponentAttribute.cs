using Detached.Modules.GraphQL.Components;
using System;

namespace Detached.Modules.GraphQL.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MutationComponentAttribute : Attribute, IComponentType
    {
        public void AddToModule(Type type, Module module)
        {
            module.Components.Add(new MutationComponent(type));
        }
    }
}