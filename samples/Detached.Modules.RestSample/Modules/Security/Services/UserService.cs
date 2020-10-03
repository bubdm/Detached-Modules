using Detached.Modules.RestSample.Modules.Security.Models;
using Detached.Modules.RestSample.Modules.Security.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.RestSample.Modules.Security.Services
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