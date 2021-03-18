using Detached.Modules.EntityFramework.Components;
using Detached.Modules.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Detached.Modules.EntityFramework
{
    public static class Package
    {
        public static void AddDbContext<TDbContext>(
            this IModule module,
            Action<DbContextOptionsBuilder> configure = null)
            where TDbContext : DbContext
        {
            module.Components.Add(new DbContextComponent<TDbContext>(configure));
        }

        public static void UseApplication(this DbContextOptionsBuilder builder, Application app)
        {
            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new DbContextOptionsExtension(app));
        }

        public static void AddRepository<TRepository>(this IModule module)
        {
            module.Components.Add(new RepositoryComponent(typeof(TRepository)));
        }

        public static void AddSeedFile<TDbContext, TEntity>(this IModule module, string filePath)
            where TDbContext : DbContext
            where TEntity : class
        {
            module.Components.Add(new SeedFileComponent<TDbContext, TEntity> { Path = filePath });
        }

        public static void AddSeedFile<TDbContext, TEntity>(this IModule module)
           where TDbContext : DbContext
           where TEntity : class
        {
            module.Components.Add(new SeedFileComponent<TDbContext, TEntity> { });
        }

        public static void AddMapping<TDbContext, TMapping>(this IModule module)
          where TDbContext : DbContext
          where TMapping : class
        {
            module.Components.Add(new MappingComponent(typeof(TDbContext), typeof(TMapping)));
        }
    }
}