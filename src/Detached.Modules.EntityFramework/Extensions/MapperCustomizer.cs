using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class MapperCustomizer : IMapperCustomizer
    {
        readonly Application _app;

        public MapperCustomizer(Application app)
        {
            _app = app;
        }

        public void Customize(DbContext dbContext, MapperOptions mapperOptions)
        {
            foreach (IComponent component in _app.Components)
            {
                ConfigureComponent(dbContext, mapperOptions, component);
            }

            foreach (Module module in _app.Modules)
            {
                foreach (IComponent component in _app.Components)
                {
                    ConfigureComponent(dbContext, mapperOptions, component);
                }
            }
        }

        void ConfigureComponent(DbContext dbContext, MapperOptions mapperOptions, IComponent component)
        {
            switch (component)
            {
                case RepositoryComponent repo:
                    if (repo.DbContextType == dbContext.GetType())
                        repo.ConfigureMapper(dbContext, mapperOptions);
                    break;
                case MappingComponent mapping:
                    if (mapping.DbContextType == dbContext.GetType())
                        mapping.ConfigureMapper(mapperOptions);
                    break;
            }
        }
    }
}
