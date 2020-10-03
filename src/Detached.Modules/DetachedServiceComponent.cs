using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules
{
    public class DetachedServiceComponent : DetachedComponent
    {
        readonly ServiceDescriptor _serviceDescriptor;

        public DetachedServiceComponent(ServiceDescriptor serviceDescriptor)
        {
            _serviceDescriptor = serviceDescriptor;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Add(_serviceDescriptor);
        }
    }
}