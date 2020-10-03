using Detached.Mappers;
using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class DetacheModulesMapperCustomizer : IMapperCustomizer
    {
        readonly DetachedApplication _app;

        public DetacheModulesMapperCustomizer(DetachedApplication app)
        {
            _app = app;
        }

        public void Customize(DbContext dbContext, MapperOptions mapperOptions)
        {
            foreach (DetachedModule module in _app.Modules)
            {
                foreach (RepositoryComponent component in module.Components.OfType<RepositoryComponent>())
                {
                    component.ConfigureMapper(dbContext, mapperOptions);
                }
            }
        }
    }
}
