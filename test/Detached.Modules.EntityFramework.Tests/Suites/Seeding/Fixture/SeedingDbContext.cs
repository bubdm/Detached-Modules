using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.Tests.Suites.Seeding.Fixture
{
    public class SeedingDbContext : DbContext
    {
        public SeedingDbContext(DbContextOptions<SeedingDbContext> options)
            : base(options)
        {
        }

        public DbSet<SeedingDocument> Documents { get; set; }
    }
}
