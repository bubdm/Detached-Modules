using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task ApplySeedFilesAsync(this DbContext dbContext)
        {
            Module module = dbContext.GetService<Module>();

            foreach (IComponent component in module.GetComponents())
            {
                if (component is SeedFileComponent dataFile && dataFile.DbContextType == dbContext.GetType())
                {
                    await dataFile.UpdateDataAsync(dbContext);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}