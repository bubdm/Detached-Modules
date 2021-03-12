using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Detached.Mappers.EntityFramework;
using Detached.Mappers;

namespace Detached.Modules.EntityFramework
{
    public class EFRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public EFRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected virtual TDbContext DbContext { get; }

        public IQueryable<TEntity> CreateQuery() 
            => DbContext.Set<TEntity>().AsNoTracking();

        public ValueTask<TEntity> FindByIdAsync(params object[] id) 
            => DbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> SaveAsync(object dto, MapperParameters mapperParams = null)
        {
            TEntity attached = await DbContext.MapAsync<TEntity>(dto, mapperParams);
            await DbContext.SaveChangesAsync();
            return attached;
        }

        public async Task DeleteAsync(object id)
        {
            DbSet<TEntity> set = DbContext.Set<TEntity>();
            TEntity entity = await set.FindAsync(id);
            set.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual void ConfigureModel(ModelBuilder modelBuilder)
        {

        }

        public virtual void ConfigureMapper(MapperOptions mapperOptions)
        {

        }
    }
}
