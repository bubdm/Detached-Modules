using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Detached.Modules
{
    public class Module : IModule
    {
        public Module()
        {
            Name = GetType().Name.Replace("Module", "");
            Version = GetType().Assembly.GetName().Version;
        }

        public string Name { get; set; }

        public Version Version { get; set; }

        public List<IComponent> Components { get; } = new List<IComponent>();

        public List<IModule> Modules { get; } = new List<IModule>();

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            foreach (IComponent component in Components)
            {
                component.ConfigureServices(this, services, configuration, hostEnvironment);
            }

            foreach (IModule module in Modules)
            {
                module.ConfigureServices(services, configuration, hostEnvironment);
            }
        }

        public IEnumerable<IComponent> GetAllComponents()
        {
            foreach (IComponent component in Components)
            {
                yield return component;
            }

            foreach (IModule module in Modules)
            {
                foreach(IComponent component in module.GetAllComponents())
                {
                    yield return component;
                }
            }
        }
    }
}