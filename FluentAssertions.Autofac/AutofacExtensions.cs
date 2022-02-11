using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Core;

[assembly:
    InternalsVisibleTo(
        "FluentAssertions.Autofac.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010001d021d48ad15ae00f3d868ab095ef510f7757370a881a21578b0a36846b78af9d8c80968157f9a44d7b9861eb914b31cbd14d5e28317ab85f4aa32b21f7006cb8cae758954081835fc6c26ce2c4965dbb664e93e02b2f5b4bbface294ed968a22a8f02832cd065f1627af932fbb41229adc042ccdc9bed67487bbb7b17d2ff6")]

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

        public static void AssertAutoActivates(this IComponentContext container, Type type)
        {
            var registration = container.ComponentRegistry.GetRegistration(type);
            AssertAutoActivates(registration, type);
        }
    }
}
