using Detached.Modules.EntityFramework;
using Detached.Modules.RestSample.Modules.Security.Models;
using Detached.Modules.RestSample.Modules.Security.Repositories;
using Detached.Modules.RestSample.Modules.Security.Services;

namespace Detached.Modules.RestSample.Modules.Security
{
    public class SecurityModule : DetachedModule
    {
        public SecurityModule()
        {
            Components.AddDataFile<MainDbContext, User>("Modules/Security/Data/UserData.json");
            Components.AddRepository<UserRepository>();
            Components.AddService<UserService>();
        }
    }
}