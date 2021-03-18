using Detached.Modules.GraphQLSample.Modules.Security.DataAccess;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using Detached.Modules.GraphQLSample.Modules.Security.Services.Input;
using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.Services
{
    public class UserMutation
    {
        public async Task<User> UserCreateAsync([Service] UserRepository userRepo, CreateUserInput createUserInput)
        {
            User user = new User();
            user.Name = createUserInput.Name;
            return await userRepo.CreateAsync(user);
        }

        public Task<User> UserThrowException()
        {
            throw new ErrorException("ThisIsErrorCode", "This is error template for: {errorName}", new Dictionary<string, object> { { "errorName", "myError" } });
        }
    }
}
