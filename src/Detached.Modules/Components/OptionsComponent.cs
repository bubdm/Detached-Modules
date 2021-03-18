using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Detached.Modules.Components
{
    public class OptionsComponent<TOptions> : IComponent
        where TOptions : class, new()
    {
        public OptionsComponent(string name = null)
        {
            Name = name ?? typeof(TOptions).Name.Replace("Options", "");
        }

        public IModule Module { get; set; }

        public string Name { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfiguration section = GetSection();
            services.Configure<TOptions>(section);
        }

        public IConfiguration GetSection()
        {
            string setting = $"{Module.Name}:{Name}";
            IConfiguration section = Module.Application.Configuration.GetSection(setting);
            return section;
        }

        public TOptions GetValue()
        {
            TOptions options = new TOptions();
            GetSection().Bind(options);
            return options;
        }
    }
}