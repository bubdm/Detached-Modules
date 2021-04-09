using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.Tests.Suites.Configuration
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
