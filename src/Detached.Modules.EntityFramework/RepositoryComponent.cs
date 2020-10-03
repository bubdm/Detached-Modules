using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Detached.Modules.EntityFramework
{
    public class RepositoryComponent : DetachedComponent
    {
        public RepositoryComponent(Type repositoryType)
        {
            RepositoryType = repositoryType;

            ConstructorInfo = repositoryType.GetConstructors().Where(c =>
            {
                var p = c.GetParameters();
                return p.Length == 1 && typeof(DbContext).IsAssignableFrom(p[0].ParameterType);
            }).FirstOrDefault();

            if (ConstructorInfo == null)
                throw new ArgumentException($"Type {repositoryType} should contain a constructor with a single parmeter of a type derived from DbContext.");

            ConfigureModelMehtodInfo = repositoryType.GetMethod("ConfigureModel");
            ConfigureMappingMethodInfo = repositoryType.GetMethod("ConfigureMapping");
        }

        public Type DbContextType { get; set; }

        public Type RepositoryType { get; set; }

        public ConstructorInfo ConstructorInfo { get; set; }

        public MethodInfo ConfigureModelMehtodInfo { get; set; }

        public MethodInfo ConfigureMappingMethodInfo { get; set; }

        public void ConfigureModel(DbContext dbContext, ModelBuilder modelBuilder)
        {
            object repoInstance = ConstructorInfo.Invoke(new[] { dbContext });

            if (ConfigureModelMehtodInfo != null)
                ConfigureModelMehtodInfo.Invoke(repoInstance, new[] { modelBuilder });
        }

        public void ConfigureMapper(DbContext dbContext, MapperOptions mapperOptions)
        {
            object repoInstance = ConstructorInfo.Invoke(new[] { dbContext });

            if (ConfigureMappingMethodInfo != null)
                ConfigureMappingMethodInfo.Invoke(repoInstance, new[] { mapperOptions });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(RepositoryType, RepositoryType, ServiceLifetime.Scoped));
        }
    }
}