using Detached.Modules.EntityFramework.Components;
using Detached.Modules.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace Detached.Modules.EntityFramework
{
    public static class Package
    {
        public static string SeedFileSuffix = "Data.json";

        public static string GetDefaultSeedFilePath(Type containerType, Type entityType)
        {
            string baseNamespace = containerType.Assembly.GetName().Name;

            StringBuilder sb = new StringBuilder(containerType.Namespace + "." + entityType.Name);
            sb.Replace(baseNamespace, "");
            sb.Replace(".", "/");
            sb.Append(SeedFileSuffix);
            sb.Insert(0, ".");

            return sb.ToString();
        }

        public static string GetDefaultSeedResourceName(Type containerType, Type entityType)
        {
            return $"{containerType.Namespace}.{entityType.Name}{SeedFileSuffix}";
        }

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

        public static void AddSeed<TSeed>(this Module module)
            where TSeed : class
        {
            module.Components.Add(new SeedComponent(typeof(TSeed)));
        }

        public static void AddMapping<TDbContext, TMapping>(this Module module)
          where TDbContext : DbContext
          where TMapping : class
        {
            module.Components.Add(new MappingComponent(typeof(TDbContext), typeof(TMapping)));
        }
    }
}