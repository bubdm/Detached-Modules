using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules
{
    public class Component
    {
        public Module Module { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {

        }
    }
}