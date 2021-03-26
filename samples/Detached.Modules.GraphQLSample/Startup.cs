using Detached.Modules.EntityFramework;
using Detached.Modules.EntityFramework.Extensions;
using Detached.Modules.GraphQL;
using Detached.Modules.GraphQLSample.Modules;
using Detached.Modules.GraphQLSample.Modules.Security;
using Detached.Modules.GraphQLSample.Modules.System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Module app = new Module { Name = "Application" };
            app.AddModule(new SecurityModule());
            app.AddModule(new SystemModule());
            app.AddDbContext<MainDbContext>(cfg =>
            {
                cfg.UseSqlite($"Data Source={Path.GetTempPath()}\\detached.db");
            });
            app.ConfigureServices(services, Configuration, HostEnvironment);

            services.AddSingleton(app);
            services.AddGraphQLServer()
                    .AddMutationType(t => t.Name("Mutation"))
                    .AddQueryType(t => t.Name("Query"))
                    .AddModule(app) // adding the module will apply all its configurations and children configurations to HotChocolate.
                    .AddFiltering()
                    .AddSorting()
                    .AddProjections();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, MainDbContext dbContext)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
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