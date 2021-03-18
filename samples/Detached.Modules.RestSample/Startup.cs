using Detached.Modules.EntityFramework;
using Detached.Modules.EntityFramework.Extensions;
using Detached.Modules.RestSample.Modules;
using Detached.Modules.RestSample.Modules.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Detached.Modules.RestSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            HostEnviornment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnviornment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IModule app = new Module { Name = "Application" };
            app.AddModule(new SecurityModule());
            app.AddDbContext<MainDbContext>(cfg =>
            {
                cfg.UseSqlite($"Data Source={Path.GetTempPath()}\\detached.db");
            });
            app.ConfigureServices(services, Configuration, HostEnviornment);

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, MainDbContext dbContext)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // don't do this in production code!
            InitializeDbAsync(dbContext).GetAwaiter().GetResult();
        }

        public async Task InitializeDbAsync(DbContext dbContext)
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.ApplySeedFilesAsync();
        }
    }
}