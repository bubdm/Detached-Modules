using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task SeedAsync(this DbContext dbContext)
        {
            Module module = dbContext.GetService<Module>();

            foreach (IComponent component in module.GetComponents())
            {
                switch(component)
                {
                    case SeedComponent seed:
                        await seed.SeedAsync(dbContext);
                        break;
                    case RepositoryComponent repo:
                        await repo.SeedAsync(dbContext);
                        break;
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}