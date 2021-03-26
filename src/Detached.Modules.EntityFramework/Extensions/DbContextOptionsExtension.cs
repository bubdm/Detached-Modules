using Detached.Mappers;
using Detached.Modules.EntityFramework.DbContextExtension;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ModelCustomizer = Detached.Modules.EntityFramework.DbContextExtension.ModelCustomizer;

namespace Detached.Modules.EntityFramework.Extensions
{
    public class DbContextOptionsExtension : IDbContextOptionsExtension
    {
        readonly Module _module;

        public DbContextOptionsExtension(Module module)
        {
            Info = new DbContextOptionsExtensionInfo(this, module);
            _module = module;
        }

        public Microsoft.EntityFrameworkCore.Infrastructure.DbContextOptionsExtensionInfo Info { get; }

        public void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(_module);
            services.AddScoped<IModelCustomizer>(sp => new ModelCustomizer(_module));
            services.AddScoped<IMapperCustomizer>(sp => new MapperCustomizer(_module));
        }

        public void Validate(IDbContextOptions options)
        {

        }
    }
}