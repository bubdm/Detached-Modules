using Detached.Mappers.EntityFramework;
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
    public class Startup : Application
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
            : base(configuration, environment)
        {
            Modules.Add(new SecurityModule());
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddControllers();

            services.AddDbContext<MainDbContext>(cfg =>
            {
                cfg.UseDetached();
                cfg.UseApplication(this);
                cfg.UseSqlite($"Data Source={Path.GetTempPath()}\\detached.db");
            });
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
            await dbContext.UpdateDataAsync();
        }
    }
}