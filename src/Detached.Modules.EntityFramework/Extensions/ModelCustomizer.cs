using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class ModelCustomizer : IModelCustomizer
    {
        readonly Module _module;

        public ModelCustomizer(Module module)
        {
            _module = module;
        }

        public void Customize(ModelBuilder modelBuilder, DbContext dbContext)
        {
            foreach (IComponent component in _module.GetComponents())
            {
                if (component is DbContextConfigurationComponent dbContextConfig)
                {
                    dbContextConfig.ConfigureModel(dbContext, modelBuilder);
                }
            }
        }
    }
}