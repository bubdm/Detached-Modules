using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Detached.Modules
{
    public class Application : IModule
    {
        public Application(IConfiguration configuration, IHostEnvironment environment)
        {
            Modules = new ModuleCollection(this);
            Components = new ComponentCollection(this);
            Configuration = configuration;
            Environment = environment;
        }

        public ModuleCollection Modules { get; }

        public IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }

        public string Name { get; set; }

        public Version Version { get; set; }

        public ComponentCollection Components { get; }

        Application IModule.Application => this;

        public void AddModule(Module module)
        {
            Modules.Add(module);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this);

            foreach (IComponent component in Components)
            {
                component.ConfigureServices(services);
            }

            foreach (Module module in Modules)
            {
                module.ConfigureServices(services);

                foreach (IComponent component in module.Components)
                {
                    component.ConfigureServices(services);
                }
            }
        }
    }
}