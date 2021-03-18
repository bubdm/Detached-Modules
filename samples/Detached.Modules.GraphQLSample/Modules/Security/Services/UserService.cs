using Detached.Modules.GraphQLSample.Modules.Security.DataAccess;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.Services
{
    public class UserService
    {
        readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepo.GetUsersAsync();
        }
    }
}