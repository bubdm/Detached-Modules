using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public abstract class SeedFileComponent : IComponent
    {
        public static string DefaultFilePath = "Modules/{ModuleName}/DataAccess/InitialData/{EntityName}Data.json";

        public IModule Module { get; set; }

        public string Path { get; set; }

        public abstract Type DbContextType { get; }

        public abstract Type EntityType { get; }

        public void ConfigureServices(IModule module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public abstract Task UpdateDataAsync(DbContext dbContext);
    }

    public class SeedFileComponent<TDbContext, TEntity> : SeedFileComponent
        where TDbContext : DbContext
        where TEntity : class
    {
        public override Type DbContextType => typeof(TDbContext);

        public override Type EntityType => typeof(TEntity);

        public override async Task UpdateDataAsync(DbContext dbContext)
        {
            string path = Path;

            if (string.IsNullOrEmpty(path))
            {
                StringBuilder pathBuilder = new StringBuilder(DefaultFilePath);
                pathBuilder.Replace("{ModuleName}", Module.Name);
                pathBuilder.Replace("{EntityName}", typeof(TEntity).Name);
                path = pathBuilder.ToString();
            }

            using (Stream fileStream = File.OpenRead(path)) 
            {
                await dbContext.ImportJsonAsync<TEntity>(fileStream);
            }
        }
    }
}