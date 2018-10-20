using System;
using System.Linq;
using Autofac;
using Autofac.Core;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("FluentAssertions.Autofac.Tests")]

namespace FluentAssertions.Autofac
{
    internal static class AutofacExtensions
    {
        public static IComponentRegistration GetRegistration<T>(this IComponentRegistry registry)
        {
            return GetRegistration(registry, typeof(T));
        }

        public static IComponentRegistration GetRegistration(this IComponentRegistry registry, Type type)
        {
            var registration = registry.Registrations
                .FirstOrDefault(r => r.Activator.LimitType == type);

            registration.Should().NotBeNull($"Type '{type}' should be registered");
            return registration;
        }

        public static void AssertAutoActivates(this IComponentRegistration registration, Type type)
        {
            registration.Services.Should()
                .Contain(service => service.Description == "AutoActivate",
                    $"Type '{type}' should be auto activated");
        }

        public static void AssertAutoActivates(this IContainer container, Type type)
        {
            var registration = container.ComponentRegistry.GetRegistration(type);
            AssertAutoActivates(registration, type);
        }
    }
}
