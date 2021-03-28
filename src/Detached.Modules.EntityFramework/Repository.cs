using Detached.Mappers;
using Detached.Mappers.EntityFramework;
using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public class Repository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public Repository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected virtual TDbContext DbContext { get; }

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

        protected async Task ImportJsonFileAsync<TEntityImport>(string filePath = null)
            where TEntityImport : class
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                string baseNamespace = GetType().Assembly.GetName().Name;

                StringBuilder sb = new StringBuilder(GetType().FullName);
                sb.Replace(baseNamespace, "");
                sb.Replace(".", "/");
                sb.Append(".json");
                sb.Insert(0, ".");

                filePath = sb.ToString();
            }

            using (Stream stream = File.OpenRead(filePath))
            {
                await DbContext.ImportJsonAsync<TEntity>(stream);
            }
        }

        protected async Task ImportJsonResourceAsync<TEntityImport>(string resourcePath = null, Assembly assembly = null)
            where TEntityImport : class
        {
            if(assembly == null)
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