using Detached.Modules.EntityFramework.DbContextExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Modules.EntityFramework
{
    public static class Package
    { 
        public static void UseApplication(this DbContextOptionsBuilder builder, DetachedApplication app)
        {
            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new DetachedModulesDbContextOptionsExtension(app));
        }

        public static void AddRepository<TRepository>(this DetachedComponentCollection components)
        {
            components.Add(new RepositoryComponent(typeof(TRepository)));
        }

        public static void AddDataFile<TDbContext, TEntity>(this DetachedComponentCollection components, string filePath)
            where TDbContext : DbContext
            where TEntity : class
        {
            components.Add(new DataFileComponent<TDbContext, TEntity> { Path = filePath });
        }

        public static void AddDataFile<TDbContext, TEntity>(this DetachedComponentCollection components)
           where TDbContext : DbContext
           where TEntity : class
        {
            components.Add(new DataFileComponent<TDbContext, TEntity> { });
        }
    }
}