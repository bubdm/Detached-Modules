using Detached.Modules.EntityFramework;
using Detached.Modules.RestSample.Modules.Security.DataAccess;
using Detached.Modules.RestSample.Modules.Security.Services;

namespace Detached.Modules.RestSample.Modules.Security
{
    public class SecurityModule : Module
    {
        public SecurityModule()
        {
            this.AddDbContextConfiguration<MainDbContext, SecurityDbContextConfiguration>();
            this.AddService<UserRepository>();
            this.AddService<UserService>();
        }
    }
}