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
        Application _app;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _app = new Application(configuration, environment);

            _app.AddModule(new SecurityModule());
            _app.AddDbContext<MainDbContext>(cfg =>
            {
                cfg.UseSqlite($"Data Source={Path.GetTempPath()}\\detached.db");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _app.ConfigureServices(services);

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