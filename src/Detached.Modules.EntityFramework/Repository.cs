using Detached.Mappers;
using Detached.Mappers.EntityFramework;
using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Detached.Modules.EntityFramework.Package;

namespace Detached.Modules.EntityFramework
{
    public abstract class Repository<TDbContext>
        where TDbContext : DbContext
    {
        public Repository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected virtual TDbContext DbContext { get; }

        public virtual void ConfigureModel(ModelBuilder modelBuilder)
        {

        }

        public virtual void ConfigureMapper(MapperOptions mapperOptions)
        {

        }

        public virtual Task SeedAsync()
        {
            return Task.CompletedTask;
        }
    }

    public abstract class Repository<TDbContext, TEntity> : Repository<TDbContext>
        where TDbContext : DbContext
        where TEntity : class
    {
        public Repository(TDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<TEntity> CreateQuery()
            => DbContext.Set<TEntity>().AsNoTracking();

        public ValueTask<TEntity> FindByIdAsync(params object[] id)
            => DbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> SaveAsync(object dto, MapperParameters mapperParams = null)
        {
            TEntity attached = await DbContext.MapAsync<TEntity>(dto, mapperParams);
            await DbContext.SaveChangesAsync();
            return attached;
        }

        public async Task DeleteAsync(object id)
        {
            DbSet<TEntity> set = DbContext.Set<TEntity>();
            TEntity entity = await set.FindAsync(id);
            set.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public override async Task SeedAsync()
        {
            string filePath = GetDefaultSeedFilePath(GetType(), typeof(TEntity));

            try
            {
                if (File.Exists(filePath))
                {
                    await ImportJsonFileAsync<TEntity>(filePath);
                }
            }
            catch (Exception x)
            {
                throw new ApplicationException($"Automatically imported seed file '{filePath}' has errors. Please fix it, delete it or override SeedAsync method in '{GetType().Name}' repository", x);
            }
        }

        protected async Task ImportJsonFileAsync<TEntityImport>(string filePath = null)
            where TEntityImport : class
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = GetDefaultSeedFilePath(GetType(), typeof(TEntity));
            }

            using (Stream stream = File.OpenRead(filePath))
            {
                await DbContext.ImportJsonAsync<TEntity>(stream);
            }
        }

        public override void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>();
        }

        protected async Task ImportJsonResourceAsync<TEntityImport>(string resourcePath = null, Assembly assembly = null)
            where TEntityImport : class
        {
            if (assembly == null)
                assembly = GetType().Assembly;

            if (string.IsNullOrEmpty(resourcePath))
                resourcePath = GetType().FullName + ".json";

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                await DbContext.ImportJsonAsync<TEntity>(stream);
            }
        }
    }
}