using System;

namespace Detached.Modules
{
    public interface IModule
    {
        string Name { get; }

        Version Version { get; }

        ComponentCollection Components { get; }

        Application Application { get; }
    }
}
