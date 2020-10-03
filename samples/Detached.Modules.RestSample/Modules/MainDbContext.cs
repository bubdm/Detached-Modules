using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.RestSample.Modules
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
