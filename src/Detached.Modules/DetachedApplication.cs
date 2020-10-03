using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Detached.Modules
{
    public class DetachedApplication
    {
        public DetachedApplication(IConfiguration configuration, IHostEnvironment environment)
        {
            Modules = new DetachedModuleCollection(this);
            Configuration = configuration;
            Environment = environment;
        }

        public DetachedModuleCollection Modules { get; }

        public IConfiguration Configuration { get; }

        public IHostEnvironment Environment { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this);

            foreach (DetachedModule module in Modules)
            {
                module.ConfigureServices(services);

                foreach (DetachedComponent component in module.Components)
                {
                    component.ConfigureServices(services);
                }
            }
        }
    }
}