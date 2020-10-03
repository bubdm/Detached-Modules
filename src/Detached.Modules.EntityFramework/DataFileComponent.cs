using Detached.Mappers;
using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public abstract class DataFileComponent : DetachedComponent
    {
        public string Path { get; set; }

        public abstract Type DbContextType { get; }

        public abstract Type EntityType { get; }

        public abstract Task UpdateDataAsync(DbContext dbContext);
    }

    public class DataFileComponent<TDbContext, TEntity> : DataFileComponent
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