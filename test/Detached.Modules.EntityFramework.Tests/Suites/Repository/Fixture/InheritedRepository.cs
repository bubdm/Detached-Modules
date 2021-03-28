using Detached.Modules.EntityFramework.Tests.Suites.Repository.Fixture;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Tests
{
    public partial class InheritedRepository : Repository<RepositoryDbContext, RepositoryDocument>
    {
        public InheritedRepository(RepositoryDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task SeedAsync()
        {
            await ImportJsonFileAsync<RepositoryDocument>();
        }
    }
}