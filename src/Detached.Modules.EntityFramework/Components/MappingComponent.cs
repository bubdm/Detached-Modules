using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Detached.Modules.EntityFramework.Components
{
    public class MappingComponent : IComponent
    {
        public MappingComponent(Type dbContextType, Type mappingType)
        {
            MapperType = mappingType;
            DbContextType = dbContextType;
            ConfigureModelMehtodInfo = mappingType.GetMethod("ConfigureModel");
            ConfigureMappingMethodInfo = mappingType.GetMethod("ConfigureMapper");
        }

        public IModule Module { get; set; }

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(MapperType, MapperType, ServiceLifetime.Scoped));
        }
    }
}