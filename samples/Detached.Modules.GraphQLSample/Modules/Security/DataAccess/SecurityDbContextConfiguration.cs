using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Annotations;
using Detached.Modules.EntityFramework.Components;
using Detached.Modules.GraphQLSample.Modules.Security.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Detached.Modules.GraphQLSample.Modules.Security.DataAccess
{
    [DbContextConfigurationComponent(typeof(MainDbContext))]
    public class SecurityDbContextConfiguration : DbContextConfiguration
    {
        public override void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
        }

        public override async Task InitializeDataAsync(DbContext dbContext)
        {
            await MapFileAsync<User>(dbContext);
            await MapFileAsync<Role>(dbContext);
        }
    }
}