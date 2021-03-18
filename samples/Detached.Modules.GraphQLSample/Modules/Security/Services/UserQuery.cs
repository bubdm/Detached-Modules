using Detached.Modules.GraphQLSample.Modules.Security.DataAccess;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.Services
{
    public class UserQuery
    {
        public async Task<IEnumerable<User>> UserGetAsync([Service]UserRepository userRepo)
        {
            return await userRepo.GetAsync();
        }
    }
}