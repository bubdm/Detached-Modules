using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules
{
    public class DetachedModule
    {
        public DetachedModule()
        {
            Components = new DetachedComponentCollection(this);
            Name = GetType().Name.Replace("Module", "");
            Version = GetType().Assembly.GetName().Version;
        }

        public string Name { get; set; }

        public Version Version { get; set; }

        public DetachedComponentCollection Components { get; } 

        public DetachedApplication Application { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {

        }
    }
}