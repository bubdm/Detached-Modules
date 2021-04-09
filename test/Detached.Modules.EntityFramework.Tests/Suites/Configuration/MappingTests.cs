using Detached.Mappers;
using Detached.Mappers.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests.Suites.Configuration
{
    public class MappingTests
    {
        [Fact]
        public void TestMappingComponent()
        {
            // GIVEN a configured application
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<TestDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestMapping?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            // GIVEN a configuration
            module.AddDbContextConfiguration<TestDbContext, TestDbContextConfiguration>();

            // WHEN the application is built
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // THEN model configuration is applied
            TestDbContext dbContext = serviceProvider.GetService<TestDbContext>();

            // get the random annotation to check that ConfigureModel was executed.
            IEntityType entityType = dbContext.Model.FindEntityType(typeof(TestDocument));
            IProperty property = entityType.FindProperty("Id");

            Assert.Contains(property.GetAnnotations(), a => a.Name == "test annotation");

            // THEN mapper configuration is applied
            ITypeOptions typeOptions = dbContext.GetInfrastructure().GetService<Mapper>().GetTypeOptions(typeof(TestDocument));
            Assert.True(typeOptions.GetMember("Name").IsKey);
        }

        public class TestDbContextConfiguration
        {
            public void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
            {
                // apply a random annotation to check that this was executed.
                modelBuilder.Entity<TestDocument>().Property(t => t.Id).HasAnnotation("test annotation", true);
            }

            public void ConfigureMapper(DbContext dbContext, MapperOptions mapperOptions)
            {
                // apply a random annotation to check that this was executed.
                mapperOptions.Configure<TestDocument>().Member(t => t.Name).IsKey();
            }
        }
    }
}