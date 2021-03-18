using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Detached.Modules.Components
{
    public class ServiceComponent : IComponent
    {
        readonly ServiceDescriptor _serviceDescriptor;

        public ServiceComponent(ServiceDescriptor serviceDescriptor)
        {
            _serviceDescriptor = serviceDescriptor;
        }

        public IModule Module { get; set; }

        public void ConfigureServices(IModule module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.Add(_serviceDescriptor);
        }
    }
}