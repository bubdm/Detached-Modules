using System;

namespace Detached.Modules
{
    public interface IComponentType
    {
        public abstract void AddToModule(Type type, Module module);
    }
}
