using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class DetachedModulesModelCustomizer : IModelCustomizer
    {
        readonly DetachedApplication _app;

        public DetachedModulesModelCustomizer(DetachedApplication app)
        {
            _app = app;
        }

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            foreach (DetachedModule module in _app.Modules)
            {
                foreach (RepositoryComponent component in module.Components.OfType<RepositoryComponent>())
                {
                    component.ConfigureModel(context, modelBuilder);
                }
            }
        }
    }
}