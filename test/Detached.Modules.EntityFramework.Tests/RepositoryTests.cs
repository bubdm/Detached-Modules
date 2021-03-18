using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Tests.Mocks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests
{
    public class RepositoryTest
    {
        [Fact]
        public void TestRepositoryMapping()
        {
            // GIVEN a configured application
            IConfiguration configuration = new ConfigurationBuilder().Build();
            IHostEnvironment hostEnvironment = new HostEnvironmentMock();

            Application app = new Application(configuration, hostEnvironment);

            // GIVEN a db context
            app.AddDbContext<TestRepositoryDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestRepository?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            // GIVEN a repository with model configuration
            app.AddRepository<TestRepository>();

            // WHEN application and services are initialized
            IServiceCollection services = new ServiceCollection();
            app.ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            TestRepositoryDbContext dbContext = serviceProvider.GetService<TestRepositoryDbContext>();

            // THEN model configuration is applied
            IEntityType entityType = dbContext.Model.FindEntityType(typeof(TestRepositoryEntity));
            IProperty property = entityType.FindProperty("Id");

            Assert.Contains(property.GetAnnotations(), a => a.Name == "test annotation");

            // THEN mapper configuration is applied
            ITypeOptions typeOptions = dbContext.GetInfrastructure().GetService<Mapper>().GetTypeOptions(typeof(TestRepositoryEntity));
            Assert.True(typeOptions.GetMember("Name").IsKey);
        }

        public class TestRepositoryEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class TestRepositoryDbContext : DbContext
        {
            public TestRepositoryDbContext(DbContextOptions<TestRepositoryDbContext> options)
                : base(options)
            {
            }

            public DbSet<TestRepositoryEntity> Entities { get; set; }
        }

        public class TestRepository
        {
            readonly TestRepositoryDbContext _testDbContext;

            public TestRepository(TestRepositoryDbContext testDbContext)
            {
                _testDbContext = testDbContext;
            }

            public void ConfigureModel(ModelBuilder modelBuilder)
            {
                // apply a random annotation to check that this was executed.
                modelBuilder.Entity<TestRepositoryEntity>().Property(t => t.Id).HasAnnotation("test annotation", true);
            }

            public void ConfigureMapper(MapperOptions mapperOptions)
            {
                // apply a random annotation to check that this was executed.
                mapperOptions.Configure<TestRepositoryEntity>().Member(t => t.Name).IsKey();
            }
        }
    }
}