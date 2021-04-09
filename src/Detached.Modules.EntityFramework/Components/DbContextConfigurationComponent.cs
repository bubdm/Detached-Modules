using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Components
{
    public class DbContextConfigurationComponent : IComponent
    {
        public DbContextConfigurationComponent(Type dbContextType, Type configurationType)
        {
            DbContextType = dbContextType;
            ConfigurationType = configurationType;
            ConfigurationObject = Activator.CreateInstance(configurationType);
        }

        public Type DbContextType { get; }

        public Type ConfigurationType { get; }

        public dynamic ConfigurationObject { get; }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            ConfigurationObject.ConfigureModel(dbContext, modelBuilder);
        }

        public void ConfigureMapper(DbContext dbContext, MapperOptions mapperOptions)
        {
            ConfigurationObject.ConfigureMapper(dbContext, mapperOptions);
        }

        public Task InitializeDataAsync(DbContext dbContext)
        {
            return ConfigurationObject.InitializeDataAsync(dbContext);
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo(
               ConfigurationType.Name,
               "DbContext Configuration (EF)",
               new Dictionary<string, object>
               {
                    { "DbContextType", DbContextType.FullName }
               }
           );
        }
    }
}
