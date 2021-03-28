using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework
{
    public class FileSeed<TDbContext, TEntity>
       where TDbContext : DbContext
       where TEntity : class
    {
        public FileSeed()
        {
            string baseNamespace = GetType().Assembly.GetName().Name;

            StringBuilder sb = new StringBuilder(GetType().FullName);
            sb.Replace(baseNamespace, "");
            sb.Replace(".", "/");
            sb.Append(".json");
            sb.Insert(0, ".");

            FilePath = sb.ToString();
        }

        public FileSeed(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }

        public virtual async Task SeedAsync(TDbContext dbContext)
        {
            using (Stream stream = File.OpenRead(FilePath))
            {
                await dbContext.ImportJsonAsync<TEntity>(stream);
            }
        }
    }
}