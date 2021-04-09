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
            this Module module,
            Action<DbContextOptionsBuilder> configure = null)
            where TDbContext : DbContext
        {
            module.Components.Add(new DbContextComponent<TDbContext>(configure));
        }

        public static void AddDbContextConfiguration<TDbContext, TConfiguration>(this Module module)
            where TDbContext : DbContext
            where TConfiguration : class
        {
            module.Components.Add(new DbContextConfigurationComponent(typeof(TDbContext), typeof(TConfiguration)));
        }

        public static void UseModule(this DbContextOptionsBuilder builder, Module module)
        {
            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new DbContextOptionsExtension(module));
        }
    }
}