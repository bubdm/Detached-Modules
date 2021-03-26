using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Detached.Modules.EntityFramework.Components
{
    public class DbContextComponent<TDbContext> : IComponent
        where TDbContext : DbContext
    {
        public DbContextComponent(Action<DbContextOptionsBuilder> configure)
        {
            Configure = configure;
        }

        public Action<DbContextOptionsBuilder> Configure { get; }

        public void ConfigureServices(Module module, IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddDbContext<TDbContext>(opts =>
            {
                Configure?.Invoke(opts);
                opts.UseDetached();
                opts.UseModule(module);
            });
        }

        public ComponentInfo GetInfo()
        {
            return new ComponentInfo(
                typeof(TDbContext).Name,
                "DbContext (EF)",
                new Dictionary<string, object>
                {
                    { "DbContextType", typeof(TDbContext).FullName }
                }
            );
        }
    }
}