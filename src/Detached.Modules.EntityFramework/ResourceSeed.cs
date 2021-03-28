using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public class ResourceSeed<TDbContext, TEntity>
           where TDbContext : DbContext
           where TEntity : class
    {
        public ResourceSeed()
        {
            Assembly = GetType().Assembly;
            ResourcePath = GetType().FullName + ".json";
        }

        public ResourceSeed(Assembly assembly, string resourcePath)
        {
            Assembly = assembly;
            ResourcePath = resourcePath;
        }

        public Assembly Assembly { get; }

        public string ResourcePath { get; }

        public virtual async Task SeedAsync(TDbContext dbContext)
        {
            using Stream stream = Assembly.GetManifestResourceStream(ResourcePath);

            if (stream == null)
                throw new ApplicationException($"Resource {ResourcePath} was not found. Make sure that the Seed class has the same namespace than the resource and Compiler Action was set to EmbeddedResource.");

            await dbContext.ImportJsonAsync<TEntity>(stream);
        }
    }
}