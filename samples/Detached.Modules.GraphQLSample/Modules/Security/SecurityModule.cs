using Detached.Modules.EntityFramework;
using Detached.Modules.GraphQL;
using Detached.Modules.GraphQLSample.Modules.Security.DataAccess;
using Detached.Modules.GraphQLSample.Modules.Security.Services;

namespace Detached.Modules.GraphQLSample.Modules.Security
{
    public class SecurityModule : Module
    {
        public SecurityModule()
        {
            AddComponents();
            this.AddRepository<UserRepository>();
            this.AddQuery<UserQuery>();
            this.AddMutation<UserMutation>();
        }
    }
}