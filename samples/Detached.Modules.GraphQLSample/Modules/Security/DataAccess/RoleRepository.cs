using Detached.Modules.Annotations;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.DataAccess
{
    [ServiceComponent]
    public class RoleRepository
    {
        readonly MainDbContext _dbContext;

        public RoleRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Role>> GetAsync()
        {
            return await _dbContext.Set<Role>().ToListAsync();
        }

        public void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>();
        }
    }
}