using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Extensions;
using Detached.Modules.EntityFramework.Tests.Suites.Repository.Fixture;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Detached.Modules.EntityFramework.Tests
{
    public partial class RepositoryTest
    {
        [Fact]
        public void TestModelConfiguration()
        {
            // GIVEN a configured module
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<RepositoryDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestRepository?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            // GIVEN a repository with model configuration
            module.AddRepository<PlainRepository>();

            // WHEN application and services are initialized
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            RepositoryDbContext dbContext = serviceProvider.GetService<RepositoryDbContext>();

            // THEN model configuration is applied
            IEntityType entityType = dbContext.Model.FindEntityType(typeof(RepositoryDocument));
            IProperty property = entityType.FindProperty("Id");

            Assert.Contains(property.GetAnnotations(), a => a.Name == "test annotation");
        }

        [Fact]
        public void TestMappingConfiguration()
        {
            // GIVEN a configured module
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<RepositoryDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestRepository?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            // GIVEN a repository with model configuration
            module.AddRepository<PlainRepository>();

            // WHEN application and services are initialized
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            RepositoryDbContext dbContext = serviceProvider.GetService<RepositoryDbContext>();

            // THEN mapper configuration is applied
            ITypeOptions typeOptions = dbContext.GetInfrastructure().GetService<Mapper>().GetTypeOptions(typeof(RepositoryDocument));
            Assert.True(typeOptions.GetMember("Name").IsKey);
        }

        [Fact]
        public async Task TestSeeding()
        {
            // GIVEN a configured module
            Module module = new Module();

            // GIVEN a db context
            module.AddDbContext<RepositoryDbContext>(cfg =>
            {
                var connection = new SqliteConnection($"DataSource=file:TestRepository?mode=memory&cache=shared");
                connection.Open();
                cfg.UseSqlite(connection);
            });

            // GIVEN a repository with model configuration
            module.AddRepository<InheritedRepository>();

            // WHEN application and services are initialized
            IServiceCollection services = new ServiceCollection();
            module.ConfigureServices(services, null, null);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            RepositoryDbContext dbContext = serviceProvider.GetService<RepositoryDbContext>();

            // AND seed is executed
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.SeedAsync();

            // THEN data is imported
            var data = await dbContext.Documents.ToListAsync();

            Assert.Contains(data, d => d.Name == "Doc1");
            Assert.Contains(data, d => d.Name == "Doc2");
            Assert.Contains(data, d => d.Name == "Doc3");
        }
    }
}