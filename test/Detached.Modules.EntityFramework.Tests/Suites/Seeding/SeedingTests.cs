using Detached.Modules.EntityFramework.Extensions;
using Detached.Modules.EntityFramework.Tests.Suites.Seeding;
using Detached.Modules.EntityFramework.Tests.Suites.Seeding.Fixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests
{
    public class SeedingTests
    {
        [Fact]
        public async Task TestResourceSeed()
        {
            // GIVEN a configured application
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<SeedingDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestSeedFiles?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            module.AddSeed<ResourceDocumentSeed>();

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // WHEN the context is set up
            SeedingDbContext dbContext = serviceProvider.GetService<SeedingDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.SeedAsync();

            // THEN file was imported
            List<SeedingDocument> imported = await dbContext.Documents.ToListAsync();
            Assert.NotNull(imported);
            Assert.Contains(imported, x => x.Id == 1);
            Assert.Contains(imported, x => x.Id == 2);
            Assert.Contains(imported, x => x.Id == 3);
        }

        [Fact]
        public async Task TestFileSeed()
        {
            // GIVEN a configured application
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<SeedingDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestSeedFiles?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            module.AddSeed<FileDocumentSeed>();

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // WHEN the context is set up
            SeedingDbContext dbContext = serviceProvider.GetService<SeedingDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.SeedAsync();

            // THEN file was imported
            List<SeedingDocument> imported = await dbContext.Documents.ToListAsync();
            Assert.NotNull(imported);
            Assert.Contains(imported, x => x.Id == 1);
            Assert.Contains(imported, x => x.Id == 2);
            Assert.Contains(imported, x => x.Id == 3);
        }
    }
}