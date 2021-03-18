using Detached.Modules.Components;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules
{
    public static class Package
    {
        public static void AddService<TService>(this IModule module, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            module.Components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TService), typeof(TService), serviceLifetime)));
        }

        public static void AddService<TContract, TService>(this IModule module, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            module.Components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TContract), typeof(TService), serviceLifetime)));
        }

        public static void AddService<TContract, TService>(this IModule module, TService instance)
        {
            module.Components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TContract), instance)));
        }

        public static void AddService<TService>(this IModule module, TService instance)
        {
            module.Components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TService), instance)));
        }

        public static void AddOptions<TOptions>(this IModule module)
            where TOptions : class, new()
        {
            module.Components.Add(new OptionsComponent<TOptions>());
        }

        public static TOptions GetOptions<TOptions>(this IModule module)
           where TOptions : class, new()
        {
            foreach (IComponent component in module.Components)
            {
                if (component is OptionsComponent<TOptions> optionsComponent)
                {
                    return optionsComponent.GetValue();
                }
            }

            throw new InvalidOperationException($"No options was registered for {typeof(TOptions).Name}.");
        }
    }
}