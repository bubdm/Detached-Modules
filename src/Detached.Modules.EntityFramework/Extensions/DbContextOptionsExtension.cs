using Detached.Mappers;
using Detached.Modules.EntityFramework.DbContextExtension;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ModelCustomizer = Detached.Modules.EntityFramework.DbContextExtension.ModelCustomizer;

namespace Detached.Modules.EntityFramework.Extensions
{
    public class DbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly Application _application;

        public DbContextOptionsExtension(Application application)
        {
            Info = new DbContextOptionsExtensionInfo(this, application);
            _application = application;
        }

        public Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo Info { get; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(_application);
            services.AddScoped<IModelCustomizer>(sp => new ModelCustomizer(_application));
            services.AddScoped<IMapperCustomizer>(sp => new MapperCustomizer(_application));
        }

        public void Validate(IDbContextOptions options)
        {

        }
    }
}