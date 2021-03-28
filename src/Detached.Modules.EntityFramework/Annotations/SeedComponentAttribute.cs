using Detached.Modules.EntityFramework.Components;
using System;

namespace Detached.Modules.EntityFramework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SeedComponentAttribute : Attribute, IComponentType
    {
        public void AddToModule(Type type, Module module)
        {
            module.Components.Add(new SeedComponent(type));
        }
    }
}