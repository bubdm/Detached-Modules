using Detached.Modules.EntityFramework.Components;
using Detached.Modules.EntityFramework.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests.Suites.Configuration
{
    public class InitialDataTests
    {
        [Fact]
        public async Task TestResourceSeed()
        {
            // GIVEN a configured application
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<TestDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestSeedFiles?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            module.AddDbContextConfiguration<TestDbContext, ResourceTestDbContextConfiguration>();

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // WHEN the context is set up
            TestDbContext dbContext = serviceProvider.GetService<TestDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.InitializeDataAsync();

            // THEN file was imported
            List<TestDocument> imported = await dbContext.Documents.ToListAsync();
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
            module.AddDbContext<TestDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestSeedFiles?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            module.AddDbContextConfiguration<TestDbContext, FileTestDbContextConfiguration>();

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // WHEN the context is set up
            TestDbContext dbContext = serviceProvider.GetService<TestDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.InitializeDataAsync();

            // THEN file was imported
            List<TestDocument> imported = await dbContext.Documents.ToListAsync();
            Assert.NotNull(imported);
            Assert.Contains(imported, x => x.Id == 1);
            Assert.Contains(imported, x => x.Id == 2);
            Assert.Contains(imported, x => x.Id == 3);
        }

        public class FileTestDbContextConfiguration : DbContextConfiguration
        {
            public override async Task InitializeDataAsync(DbContext dbContext)
            {
                await MapFileAsync<TestDocument>(dbContext);
            }
        }

        public class ResourceTestDbContextConfiguration : DbContextConfiguration
        {
            public override async Task InitializeDataAsync(DbContext dbContext)
            {
                await MapResourceAsync<TestDocument>(dbContext);
            }
        }
    }
}