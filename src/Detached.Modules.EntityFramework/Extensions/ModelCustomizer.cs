using Detached.Modules.EntityFramework.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class ModelCustomizer : IModelCustomizer
    {
        readonly Application _app;

        public ModelCustomizer(Application app)
        {
            _app = app;
        }

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            foreach (IComponent component in _app.Components)
            {
                ConfigureComponent(modelBuilder, context, component);
            }

            foreach (Module module in _app.Modules)
            {
                foreach (IComponent component in _app.Components)
                {
                    ConfigureComponent(modelBuilder, context, component);
                }
            }
        }

        void ConfigureComponent(ModelBuilder modelBuilder, DbContext context, IComponent component)
        {
            switch (component)
            {
                case RepositoryComponent repo:
                    if (repo.DbContextType == context.GetType())
                        repo.ConfigureModel(context, modelBuilder);
                    break;
                case MappingComponent mapping:
                    if (mapping.DbContextType == context.GetType())
                        mapping.ConfigureModel(modelBuilder);
                    break;
            }
        }
    }
}