using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class MapperCustomizer : IMapperCustomizer
    {
        readonly IModule _module;

        public MapperCustomizer(IModule module)
        {
            _module = module;
        }

        public void Customize(DbContext dbContext, MapperOptions mapperOptions)
        {
            foreach (IComponent component in _module.GetAllComponents())
            {
                switch (component)
                {
                    case RepositoryComponent repo:
                        if (repo.DbContextType == dbContext.GetType())
                        {
                            repo.ConfigureMapper(dbContext, mapperOptions);
                        }
                        break;
                    case MappingComponent mapping:
                        if (mapping.DbContextType == dbContext.GetType())
                        {
                            mapping.ConfigureMapper(mapperOptions);
                        }
                        break;
                }
            }
        }
    }
}
