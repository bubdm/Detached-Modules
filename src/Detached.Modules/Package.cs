using Detached.Modules.Components;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules
{
    public static class Package
    {
        public static void AddService<TService>(this ComponentCollection components, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TService), typeof(TService), serviceLifetime)));
        }

        public static void AddService<TContract, TService>(this ComponentCollection components, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TContract), typeof(TService), serviceLifetime)));
        }

        public static void AddService<TContract, TService>(this ComponentCollection components, TService instance)
        {
            components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TContract), instance)));
        }

        public static void AddService<TService>(this ComponentCollection components, TService instance)
        {
            components.Add(new ServiceComponent(new ServiceDescriptor(typeof(TService), instance)));
        }

        public static void AddOptions<TOptions>(this ComponentCollection components)
            where TOptions : class, new()
        {
            components.Add(new OptionsComponent<TOptions>());
        }

        public static TOptions GetOptions<TOptions>(this ComponentCollection components)
           where TOptions : class, new()
        {
            foreach (Component component in components)
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