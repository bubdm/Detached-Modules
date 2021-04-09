using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task InitializeDataAsync(this DbContext dbContext)
        {
            Module module = dbContext.GetService<Module>();

            foreach (IComponent component in module.GetComponents())
            {
                if (component is DbContextConfigurationComponent dbContextConfig)
                {
                    await dbContextConfig.InitializeDataAsync(dbContext);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}