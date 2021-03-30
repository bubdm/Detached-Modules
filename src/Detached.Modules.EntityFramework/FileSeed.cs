using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using static Detached.Modules.EntityFramework.Package;

namespace Detached.Modules.EntityFramework
{
    public class FileSeed<TDbContext, TEntity>
       where TDbContext : DbContext
       where TEntity : class
    {
        public FileSeed()
        {
            FilePath = GetDefaultSeedFilePath(GetType(), typeof(TEntity));
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