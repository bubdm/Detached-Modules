using Detached.Mappers.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Detached.Modules.EntityFramework.Components
{
    public class SeedComponent : IComponent
    {
        public SeedComponent(Type seederType)
        {
            SeederType = seederType;
            SeedAsyncMethodInfo = seederType.GetMethod("SeedAsync", BindingFlags.Instance |  BindingFlags.Public | BindingFlags.NonPublic);
            if (SeedAsyncMethodInfo == null)
                throw new ArgumentException($"Type {seederType} doesn't have a method called SeedAsync");

            var parameters = SeedAsyncMethodInfo.GetParameters();
            if (parameters.Length != 1 || !typeof(DbContext).IsAssignableFrom(parameters[0].ParameterType))
                throw new ArgumentException($"Method SeedAsync in type {seederType} should contain one argument of a type derived from DbContext.");

            DbContextType = parameters[0].ParameterType;            
        }

        public Type DbContextType { get; set; }

        public Type SeederType { get; set; } 

        public MethodInfo SeedAsyncMethodInfo { get; set; }

        public async Task SeedAsync(DbContext dbContext)
        {
            object repoInstance = Activator.CreateInstance(SeederType);

            if (SeedAsyncMethodInfo != null)
            {
                await (Task)SeedAsyncMethodInfo.Invoke(repoInstance, new[] { dbContext });
            }
        }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo(
                SeederType.Name,
                "Seed (EF)",
                new Dictionary<string, object>
                {
                    { nameof(SeederType), SeederType.FullName },
                    { nameof(DbContextType), DbContextType.FullName }
                }
            );
        }
    }
}