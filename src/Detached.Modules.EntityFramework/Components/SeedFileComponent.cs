using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public abstract class SeedFileComponent : IComponent
    {
        public string Path { get; set; }

        public abstract Type DbContextType { get; }

        public abstract Type EntityType { get; }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo(
                Path,
                "SeedFile (EF)",
                new Dictionary<string, object>
                {
                    { nameof(DbContextType), DbContextType.FullName },
                    { nameof(EntityType), EntityType.FullName }
                });
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
            using (Stream fileStream = File.OpenRead(Path))
            {
                await dbContext.ImportJsonAsync<TEntity>(fileStream);
            }
        }
    }
}