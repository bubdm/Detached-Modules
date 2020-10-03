using Detached.Mappers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class DetachedModulesDbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly DetachedApplication _application;

        public DetachedModulesDbContextOptionsExtension(DetachedApplication application)
        {
            Info = new DetachedModulesDbContextOptionsExtensionInfo(this);
            _application = application;
        }

        public DbContextOptionsExtensionInfo Info { get; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(_application);
            services.AddScoped<IModelCustomizer>(sp => new DetachedModulesModelCustomizer(_application));
            services.AddScoped<IMapperCustomizer>(sp => new DetacheModulesMapperCustomizer(_application));
        }

        public void Validate(IDbContextOptions options)
        {

        }
    }
}