using Detached.Modules.EntityFramework.Components;
using Detached.Modules.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.EntityFramework
{
    public static class Package
    {
        public static void AddDbContext<TDbContext>(
            this Module module,
            Action<DbContextOptionsBuilder> configure = null)
            where TDbContext : DbContext
        {
            module.Components.Add(new DbContextComponent<TDbContext>(configure));
        }

        public static void UseModule(this DbContextOptionsBuilder builder, Module module)
        {
            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new DbContextOptionsExtension(module));
        }

        public static void AddRepository<TRepository>(this Module module, Type contractType = null, Type dbContetType = null, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            module.Components.Add(new RepositoryComponent(typeof(TRepository), dbContetType, contractType, lifetime));
        }

        public static void AddSeedFile<TDbContext, TEntity>(this Module module, string filePath)
            where TDbContext : DbContext
            where TEntity : class
        {
            module.Components.Add(new SeedFileComponent<TDbContext, TEntity> { Path = filePath });
        }

        public static void AddSeedFile<TDbContext, TEntity>(this Module module)
           where TDbContext : DbContext
           where TEntity : class
        {
            module.Components.Add(new SeedFileComponent<TDbContext, TEntity> { });
        }

        public static void AddMapping<TDbContext, TMapping>(this Module module)
          where TDbContext : DbContext
          where TMapping : class
        {
            module.Components.Add(new MappingComponent(typeof(TDbContext), typeof(TMapping)));
        }
    }
}