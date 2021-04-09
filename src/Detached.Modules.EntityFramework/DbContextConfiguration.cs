using Detached.Mappers.EntityFramework;
using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Components
{
    public class DbContextConfiguration
    {
        public static string SeedFileSuffix = "Data.json";

        public virtual void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
        {

        }

        public virtual void ConfigureMapper(DbContext dbContext, MapperOptions mapperOptions)
        {

        }

        public virtual Task InitializeDataAsync(DbContext dbContext)
        {
            return Task.CompletedTask;
        }

        public async Task MapFileAsync<TEntity>(DbContext dbContext, string filePath = null)
            where TEntity : class
        {
            if (filePath == null)
            {
                string baseNamespace = GetType().Assembly.GetName().Name;

                StringBuilder sb = new StringBuilder(GetType().Namespace + "." + typeof(TEntity).Name);
                sb.Replace(baseNamespace, "");
                sb.Replace(".", "/");
                sb.Append(SeedFileSuffix);
                sb.Insert(0, ".");

                filePath = sb.ToString();
            }

            using (Stream fileStream = File.OpenRead(filePath))
            {
                await dbContext.ImportJsonAsync<TEntity>(fileStream);
            }
        }

        public async Task MapResourceAsync<TEntity>(DbContext dbContext, Assembly assembly = null, string resourceName = null)
            where TEntity : class
        {
            if (assembly == null)
            {
                assembly = GetType().Assembly;
            }

            if (resourceName == null)
            {
                resourceName = $"{GetType().Namespace}.{typeof(TEntity).Name}{SeedFileSuffix}";
            }

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                await dbContext.ImportJsonAsync<TEntity>(resourceStream);
            }
        }
    }
}