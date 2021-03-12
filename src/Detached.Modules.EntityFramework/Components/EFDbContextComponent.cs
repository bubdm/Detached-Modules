using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules.EntityFramework.Components
{
    public class EFDbContextComponent<TDbContext> : Component
        where TDbContext : DbContext
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext>();
        }
    }
}