using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public abstract class EFSeedFileComponent : Component
    {
        public string Path { get; set; }

        public abstract Type DbContextType { get; }

        public abstract Type EntityType { get; }

        public abstract Task UpdateDataAsync(DbContext dbContext);
    }

    public class EFSeedFileComponent<TDbContext, TEntity> : EFSeedFileComponent
        where TDbContext : DbContext
        where TEntity : class
    {
        public override Type DbContextType => typeof(TDbContext);

        public override Type EntityType => typeof(TEntity);

        public override async Task UpdateDataAsync(DbContext dbContext)
        {
            string path = Path;
            if (path == null)
                path = $"Modules/{Module.Name}/DataAccess/InitialData/{typeof(TEntity).Name}Data.json";

            using (Stream fileStream = File.OpenRead(path))
            {
                await dbContext.ImportJsonAsync<TEntity>(fileStream);
            }
        }
    }
}