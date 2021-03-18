using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class ModelCustomizer : IModelCustomizer
    {
        readonly IModule _module;

        public ModelCustomizer(IModule module)
        {
            _module = module;
        }

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            foreach (IComponent component in _module.GetAllComponents())
            {
                switch (component)
                {
                    case RepositoryComponent repo:
                        if (repo.DbContextType == context.GetType())
                        {
                            repo.ConfigureModel(context, modelBuilder);
                        }
                        break;
                    case MappingComponent mapping:
                        if (mapping.DbContextType == context.GetType())
                        {
                            mapping.ConfigureModel(modelBuilder);
                        }
                        break;
                }
            }
        }
    }
}