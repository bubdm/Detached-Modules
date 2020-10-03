using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task UpdateDataAsync(this DbContext dbContext)
        {
            DetachedApplication app = dbContext.GetService<DetachedApplication>();
            JsonSerializerOptions jsonSerializerOptions = dbContext.GetService<JsonSerializerOptions>();

            foreach (DetachedModule module in app.Modules)
            {
                foreach (DetachedComponent component in module.Components)
                {
                    if (component is DataFileComponent dataFile
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