using Detached.Mappers.EntityFramework;
using Detached.Modules.EntityFramework.Annotations;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using System.IO;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.DataAccess
{
    [SeedComponent]
    public class RoleSeed
    {
        public async Task SeedAsync(MainDbContext dbContext)
        {
            using (Stream stream = File.OpenRead("Modules/Security/DataAccess/RoleSeed.json"))
            {
                await dbContext.ImportJsonAsync<Role>(stream);
            }
        }
    }
}