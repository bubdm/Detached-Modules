using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.GraphQLSample.Modules
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
