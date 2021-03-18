using Detached.Mappers.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules.EntityFramework.Components
{
    public class DbContextComponent<TDbContext> : IComponent
        where TDbContext : DbContext
    {
        public DbContextComponent(Action<DbContextOptionsBuilder> configure)
        {
            Configure = configure;
        }

        public IModule Module { get; set; }

        public Action<DbContextOptionsBuilder> Configure { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TDbContext>(opts =>
            {
                Configure?.Invoke(opts);
                opts.UseDetached();
                opts.UseApplication(Module.Application);
            });
        }
    }
}