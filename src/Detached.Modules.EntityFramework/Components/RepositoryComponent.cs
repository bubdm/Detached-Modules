using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Detached.Modules.EntityFramework
{
    public class RepositoryComponent : IComponent
    {
        public RepositoryComponent(Type repositoryType, Type dbContextType, Type contractType, ServiceLifetime? lifeTime)
        {
            RepositoryType = repositoryType;
            DbContextType = dbContextType;
            ContractType = contractType;
            LifeTime = lifeTime ?? ServiceLifetime.Scoped;

            if (ContractType == null)
                ContractType = RepositoryType;

            ConstructorInfo = repositoryType.GetConstructors().Where(c =>
            {
                var p = c.GetParameters();
                return p.Length == 1 && typeof(DbContext).IsAssignableFrom(p[0].ParameterType);
            }).FirstOrDefault();

            if (ConstructorInfo == null && DbContextType == null)
                throw new ArgumentException($"Type {repositoryType} should contain a constructor with a single parmeter of a type derived from DbContext.");

            if (DbContextType == null)
                DbContextType = ConstructorInfo.GetParameters()[0].ParameterType;

            ConfigureModelMehtodInfo = repositoryType.GetMethod("ConfigureModel");
            ConfigureMappingMethodInfo = repositoryType.GetMethod("ConfigureMapper");
        }

        public Module Module { get; set; }

        public Type DbContextType { get; set; }

        public Type RepositoryType { get; set; }

        public ServiceLifetime LifeTime { get; set; } = ServiceLifetime.Scoped;

        public Type ContractType { get; set; }

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

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.Add(new ServiceDescriptor(RepositoryType, ContractType ?? RepositoryType, LifeTime));
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo(
                RepositoryType.Name,
                "Repository (EF)",
                new Dictionary<string, object>
                {
                    { nameof(DbContextType), DbContextType.FullName },
                    { nameof(RepositoryType), RepositoryType.FullName },
                    { nameof(ContractType), ContractType.FullName },
                    { "HasModelConfiguration", ConfigureModelMehtodInfo != null },
                    { "HasMappingConfiguration", ConfigureModelMehtodInfo != null }
                }
            );
        }
    }
}