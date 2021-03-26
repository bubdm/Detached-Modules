using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.EntityFramework.Annotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RepositoryComponentAttribute : Attribute, IComponentType
    {
        public ServiceLifetime LifeTime { get; set; } = ServiceLifetime.Scoped;

        public Type ContractType { get; set; }

        public Type DbContextType { get; set; }

        public void AddToModule(Type type, Module module)
        {
            module.Components.Add(new RepositoryComponent(type, DbContextType, ContractType, LifeTime));
        }
    }
}
