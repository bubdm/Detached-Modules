using Detached.Modules.GraphQL.Annotations;
using Detached.Modules.GraphQLSample.Modules.Security.DataAccess;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.Services
{
    [QueryComponent]
    public class RoleQuery
    {
        public Task<IEnumerable<Role>> RoleGetAsync([Service]RoleRepository roleRepo)
        {
            return roleRepo.GetAsync();
        }
    }
}