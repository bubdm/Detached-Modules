using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Detached.Modules
{
    public interface IModule
    {
        string Name { get; }

        Version Version { get; }

        List<IComponent> Components { get; }

        List<IModule> Modules { get; }

        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment);

        IEnumerable<IComponent> GetAllComponents();
    }
}