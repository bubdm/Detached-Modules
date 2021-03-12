using Detached.Modules.EntityFramework.DbContextExtension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Modules.EntityFramework
{
    public static class Package
    { 
        public static void AddDbContext(this ComponentCollection components)
        {

        }

        public static void UseApplication(this DbContextOptionsBuilder builder, Application app)
        {
            ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(new ModulesDbContextOptionsExtension(app));
        }

        public static void AddRepository<TRepository>(this ComponentCollection components)
        {
            components.Add(new EFRepositoryComponent(typeof(TRepository)));
        }

        public static void AddSeedFile<TDbContext, TEntity>(this ComponentCollection components, string filePath)
            where TDbContext : DbContext
            where TEntity : class
        {
            components.Add(new EFSeedFileComponent<TDbContext, TEntity> { Path = filePath });
        }

        public static void AddSeedFile<TDbContext, TEntity>(this ComponentCollection components)
           where TDbContext : DbContext
           where TEntity : class
        {
            components.Add(new EFSeedFileComponent<TDbContext, TEntity> { });
        }
    }
}