using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules
{
    public class Module : IModule
    {
        public Module()
        {
            Components = new ComponentCollection(this);
            Name = GetType().Name.Replace("Module", "");
            Version = GetType().Assembly.GetName().Version;
        }

        public string Name { get; set; }

        public Version Version { get; set; }

        public ComponentCollection Components { get; } 

        public Application Application { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {

        }
    }
}