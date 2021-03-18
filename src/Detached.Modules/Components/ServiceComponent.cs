using Microsoft.Extensions.DependencyInjection;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(_serviceDescriptor);
        }
    }
}