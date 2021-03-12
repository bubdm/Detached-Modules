using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task UpdateDataAsync(this DbContext dbContext)
        {
            Application app = dbContext.GetService<Application>();

            foreach (Module module in app.Modules)
            {
                foreach (Component component in module.Components)
                {
                    if (component is EFSeedFileComponent dataFile
                       && dataFile.DbContextType == dbContext.GetType())
                    {
                        await dataFile.UpdateDataAsync(dbContext);
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}