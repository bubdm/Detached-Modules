using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Tests.Suites.Repository.Fixture;
using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.Tests.Suites.Repository.Fixture
{
    public partial class PlainRepository
    {
        readonly RepositoryDbContext _testDbContext;

        public PlainRepository(RepositoryDbContext testDbContext)
        {
            _testDbContext = testDbContext;
        }

        public void ConfigureModel(ModelBuilder modelBuilder)
        {
            // apply a random annotation to check that this was executed.
            modelBuilder.Entity<RepositoryDocument>().Property(t => t.Id).HasAnnotation("test annotation", true);
        }

        public void ConfigureMapper(MapperOptions mapperOptions)
        {
            // apply a random annotation to check that this was executed.
            mapperOptions.Configure<RepositoryDocument>().Member(t => t.Name).IsKey();
        }
    }
}