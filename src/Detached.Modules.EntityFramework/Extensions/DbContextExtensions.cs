using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task ApplySeedFilesAsync(this DbContext dbContext)
        {
            Application app = dbContext.GetService<Application>();

            foreach (IComponent component in app.Components)
            {
                await ConfigureComponent(dbContext, component);
            }

            foreach (Module module in app.Modules)
            {
                foreach (IComponent component in module.Components)
                {
                    await ConfigureComponent(dbContext, component);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        static async Task ConfigureComponent(DbContext dbContext, IComponent component)
        {
            if (component is SeedFileComponent dataFile && dataFile.DbContextType == dbContext.GetType())
            {
                await dataFile.UpdateDataAsync(dbContext);
            }
        }
    }
}