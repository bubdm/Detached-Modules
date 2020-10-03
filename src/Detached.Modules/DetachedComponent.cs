using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules
{
    public class DetachedComponent
    {
        public DetachedModule Module { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {

        }
    }
}