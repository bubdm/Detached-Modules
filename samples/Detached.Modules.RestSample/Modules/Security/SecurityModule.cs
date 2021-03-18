using Detached.Modules.EntityFramework;
using Detached.Modules.RestSample.Modules.Security.DataAccess;
using Detached.Modules.RestSample.Modules.Security.Models;
using Detached.Modules.RestSample.Modules.Security.Services;

namespace Detached.Modules.RestSample.Modules.Security
{
    public class SecurityModule : Module
    {
        public SecurityModule()
        {
            this.AddSeedFile<MainDbContext, User>("Modules/Security/DataAccess/UserData.json");
            this.AddRepository<UserRepository>();
            this.AddService<UserService>();
        }
    }
}