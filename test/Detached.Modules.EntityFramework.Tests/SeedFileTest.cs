using Detached.Modules.EntityFramework.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests
{
    public class SeedFileTest
    {
        [Fact]
        public async Task TestSeedFiles()
        {
            // GIVEN a configured application
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<TestSeedFileDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestSeedFiles?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            module.AddSeedFile<TestSeedFileDbContext, TestSeedFileEntity>("SeedFile.json");

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // WHEN the context is set up
            TestSeedFileDbContext dbContext = serviceProvider.GetService<TestSeedFileDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.ApplySeedFilesAsync();

            // THEN file was imported
            List<TestSeedFileEntity> imported = await dbContext.Entities.ToListAsync();
            Assert.NotNull(imported);
            Assert.Contains(imported, x => x.Id == 1);
            Assert.Contains(imported, x => x.Id == 2);
            Assert.Contains(imported, x => x.Id == 3);
        }

        public class TestSeedFileEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class TestSeedFileDbContext : DbContext
        {
            public TestSeedFileDbContext(DbContextOptions<TestSeedFileDbContext> options)
                : base(options)
            {
            }

            public DbSet<TestSeedFileEntity> Entities { get; set; }
        }
    }
}