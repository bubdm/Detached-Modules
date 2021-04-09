using Detached.Modules.EntityFramework.Components;
using Detached.Modules.RestSample.Modules.Security.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Detached.Modules.RestSample.Modules.Security.DataAccess
{
    public class SecurityDbContextConfiguration : DbContextConfiguration
    {
        public override void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }

        public override async Task InitializeDataAsync(DbContext dbContext)
        {
            await MapFileAsync<User>(dbContext);
        }
    }
}