using Detached.Modules.EntityFramework.Tests.Suites.Repository.Fixture;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Tests.Suites.Repository.Fixture
{
    public partial class InheritedRepository : Repository<TestDbContext, TestDocument>
    {
        public InheritedRepository(TestDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}