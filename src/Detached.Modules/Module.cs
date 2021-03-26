using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Detached.Modules
{
    public class Module
    {
        public Module()
        {
            Name = GetType().Name.Replace("Module", "");
            Version = GetType().Assembly.GetName().Version;
        }

        public string Name { get; set; }

        public Version Version { get; set; }

        public List<IComponent> Components { get; } = new List<IComponent>();

        public List<Module> Modules { get; } = new List<Module>();

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            foreach (IComponent component in Components)
            {
                component.ConfigureServices(this, services, configuration, hostEnvironment);
            }

            foreach (Module module in Modules)
            {
                module.ConfigureServices(services, configuration, hostEnvironment);
            }
        }

        public IEnumerable<IComponent> GetComponents()
        {
            foreach (IComponent component in Components)
            {
                yield return component;
            }

            foreach (Module submodule in Modules)
            {
                foreach (IComponent component in submodule.GetComponents())
                {
                    yield return component;
                }
            }
        }

        /// <summary>
        /// Adds all components in the current namespace and descendant namespaces.
        /// </summary>
        public void AddComponents()
        {
            AddComponents(t => t.Namespace.StartsWith(GetType().Namespace), GetType().Assembly);
        }

        public void AddComponents(Func<Type, bool> filter = null, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = GetType().Assembly;

            foreach (Type type in assembly.DefinedTypes)
            {
                IComponentType annotation = GetComponentType(type);
                if (annotation != null)
                {
                    if (filter(type))
                    {
                        annotation.AddToModule(type, this);
                    }
                }
            }
        }

        public void AddComponent(Type type)
        {
            IComponentType annotation = GetComponentType(type);
            if (annotation == null)
                throw new ArgumentException($"Type {type} does not provide component information.");

            annotation.AddToModule(type, this);
        }

        public void AddModule(Module module)
        {
            Modules.Add(module);
        }

        public IComponentType GetComponentType(Type type)
        {
            return type.GetCustomAttributes().OfType<IComponentType>().FirstOrDefault();
        }

        public ModuleInfo GetInfo()
        {
            ModuleInfo[] submodules = new ModuleInfo[Modules.Count];
            for (int i = 0; i < submodules.Length; i++)
            {
                submodules[i] = Modules[i].GetInfo();
            }

            ComponentInfo[] components = new ComponentInfo[Components.Count];
            for (int i = 0; i < components.Length; i++)
            {
                components[i] = Components[i].GetInfo();
            }

            return new ModuleInfo(Name, Version.ToString(), submodules, components);
        }
    }
}