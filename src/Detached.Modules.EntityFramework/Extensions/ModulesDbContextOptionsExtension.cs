using Detached.Mappers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class ModulesDbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly Application _application;

        public ModulesDbContextOptionsExtension(Application application)
        {
            Info = new ModulesDbContextOptionsExtensionInfo(this, application);
            _application = application;
        }

        public DbContextOptionsExtensionInfo Info { get; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(_application);
            services.AddScoped<IModelCustomizer>(sp => new ModulesModelCustomizer(_application));
            services.AddScoped<IMapperCustomizer>(sp => new ModulesMapperCustomizer(_application));
        }

        public void Validate(IDbContextOptions options)
        {

        }
    }
}