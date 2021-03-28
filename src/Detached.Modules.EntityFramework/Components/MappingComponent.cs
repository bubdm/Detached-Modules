using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Detached.Modules.EntityFramework.Components
{
    public class MappingComponent : IComponent
    {
        public MappingComponent(Type dbContextType, Type mappingType)
        {
            MapperType = mappingType;
            DbContextType = dbContextType;
            ConfigureModelMehtodInfo = mappingType.GetMethod("ConfigureModel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            ConfigureMappingMethodInfo = mappingType.GetMethod("ConfigureMapper", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public Type DbContextType { get; set; }

        public Type MapperType { get; set; }

        public ConstructorInfo ConstructorInfo { get; set; }

        public MethodInfo ConfigureModelMehtodInfo { get; set; }

        public MethodInfo ConfigureMappingMethodInfo { get; set; }

        public void ConfigureModel(ModelBuilder modelBuilder)
        {
            object repoInstance = Activator.CreateInstance(MapperType);

            if (ConfigureModelMehtodInfo != null)
                ConfigureModelMehtodInfo.Invoke(repoInstance, new[] { modelBuilder });
        }

        public void ConfigureMapper(MapperOptions mapperOptions)
        {
            object repoInstance = Activator.CreateInstance(MapperType);

            if (ConfigureMappingMethodInfo != null)
                ConfigureMappingMethodInfo.Invoke(repoInstance, new[] { mapperOptions });
        }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo(
                MapperType.Name,
                "Mapping (EF)",
                new Dictionary<string, object>
                {
                    { nameof(MapperType), MapperType.FullName },
                    { nameof(DbContextType), DbContextType.FullName }
                }
            );
        }
    }
}