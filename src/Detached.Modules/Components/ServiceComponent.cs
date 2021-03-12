using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules.Components
{
    public class ServiceComponent : Component
    {
        readonly ServiceDescriptor _serviceDescriptor;

        public ServiceComponent(ServiceDescriptor serviceDescriptor)
        {
            _serviceDescriptor = serviceDescriptor;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Add(_serviceDescriptor);
        }
    }
}