using Microsoft.Extensions.DependencyInjection;
using System;

namespace Detached.Modules
{
    public static class Package
    {
        public static void AddService<TService>(this DetachedComponentCollection components, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            components.Add(new DetachedServiceComponent(new ServiceDescriptor(typeof(TService), typeof(TService), serviceLifetime)));
        }

        public static void AddService<TContract, TService>(this DetachedComponentCollection components, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            components.Add(new DetachedServiceComponent(new ServiceDescriptor(typeof(TContract), typeof(TService), serviceLifetime)));
        }

        public static void AddService<TContract, TService>(this DetachedComponentCollection components, TService instance)
        {
            components.Add(new DetachedServiceComponent(new ServiceDescriptor(typeof(TContract), instance)));
        }

        public static void AddService<TService>(this DetachedComponentCollection components, TService instance)
        {
            components.Add(new DetachedServiceComponent(new ServiceDescriptor(typeof(TService), instance)));
        }

        public static void AddOptions<TOptions>(this DetachedComponentCollection components)
            where TOptions : class, new()
        {
            components.Add(new DetachedOptionsComponent<TOptions>());
        }

        public static TOptions GetOptions<TOptions>(this DetachedComponentCollection components)
           where TOptions : class, new()
        {
            foreach (DetachedComponent component in components)
            {
                if (component is DetachedOptionsComponent<TOptions> optionsComponent)
                {
                    return optionsComponent.GetValue();
                }
            }

            throw new InvalidOperationException($"No options was registered for {typeof(TOptions).Name}.");
        }
    }
}