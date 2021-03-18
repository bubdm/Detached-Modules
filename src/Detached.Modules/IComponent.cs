using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules
{
    public interface IComponent
    {
        public IModule Module { get; set; }

        void ConfigureServices(IServiceCollection services)
        {

        }
    }
}