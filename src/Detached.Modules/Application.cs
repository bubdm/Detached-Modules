using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Detached.Modules
{
    public class Application
    {
        public Application(IConfiguration configuration, IHostEnvironment environment)
        {
            Modules = new ModuleCollection(this);
            Configuration = configuration;
            Environment = environment;
        }

        public ModuleCollection Modules { get; }

        public IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this);

            foreach (Module module in Modules)
            {
                module.ConfigureServices(services);

                foreach (Component component in module.Components)
                {
                    component.ConfigureServices(services);
                }
            }
        }
    }
}