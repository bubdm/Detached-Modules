using Detached.Mappers;
using Detached.Mappers.Model;
using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class MappingCustomizer : IMapperCustomizer
    {
        readonly Module _module;

        public MappingCustomizer(Module module)
        {
            _module = module;
        }

        public void Customize(DbContext dbContext, MapperOptions mapperOptions)
        {
            foreach (IComponent component in _module.GetComponents())
            {
                if (component is DbContextConfigurationComponent dbContextConfig)
                {
                    dbContextConfig.ConfigureMapper(dbContext, mapperOptions);
                }
            }
        }
    }
}
