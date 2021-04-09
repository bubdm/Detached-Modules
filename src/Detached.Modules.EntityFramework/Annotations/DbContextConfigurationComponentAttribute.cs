using Detached.Modules.EntityFramework.Components;
using System;

namespace Detached.Modules.EntityFramework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DbContextConfigurationComponentAttribute : Attribute, IComponentType
    {
        public DbContextConfigurationComponentAttribute(Type dbContextType)
        {
            DbContextType = dbContextType;
        }

        public Type DbContextType { get; }

        public void AddToModule(Type type, Module module)
        {
            module.Components.Add(new DbContextConfigurationComponent(DbContextType, type));
        }
    }
}