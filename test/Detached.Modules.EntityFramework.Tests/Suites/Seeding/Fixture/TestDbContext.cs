using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.Tests.Suites.Seeding.Fixture
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestDocument> Documents { get; set; }
    }
}
