using System;
using System.Diagnostics.CodeAnalysis;
using Autofac.Core;
using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    [TestFixtureFor(typeof(RegistrationAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegistrationAssertions_Should
    {
        [Test]
        public void Register_Named()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterType<NamedDummy>().Named<ICloneable>("Dummy")
                );

            containerShouldHave.Registered<NamedDummy>()
                .Named<ICloneable>("Dummy")
                .Keyed<ICloneable>("Dummy");
        }

        [Test]
        public void Register_Keyed()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterType<KeyedDummy>().Keyed<IComparable>(42)
                );

            containerShouldHave.Registered<KeyedDummy>()
                .Keyed<IComparable>(42);
        }

        [Test]
        public void Register_Lifetime()
        {
            var containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().SingleInstance());
            containerShouldHave.Registered<Dummy>().SingleInstance();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerDependency());
            containerShouldHave.Registered<Dummy>().InstancePerDependency();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerLifetimeScope());
            containerShouldHave .Registered<Dummy>().InstancePerLifetimeScope();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerMatchingLifetimeScope());
            containerShouldHave .Registered<Dummy>().InstancePerMatchingLifetimeScope();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerRequest());
            containerShouldHave .Registered<Dummy>().InstancePerRequest();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerOwned<ICloneable>());
            containerShouldHave .Registered<Dummy>().InstancePerOwned<ICloneable>();
        }

        [Test]
        public void Register_Ownership()
        {
            var containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().ExternallyOwned());
            containerShouldHave .Registered<Dummy>()
                .ExternallyOwned()
                .Owned(InstanceOwnership.ExternallyOwned);

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().OwnedByLifetimeScope());
            containerShouldHave .Registered<Dummy>()
                .OwnedByLifetimeScope()
                .Owned(InstanceOwnership.OwnedByLifetimeScope);
        }

        [Test]
        public void Register_AutoActivate()
        {
            var containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().As<IDisposable>().AutoActivate());
            containerShouldHave .Registered<Dummy>().As<IDisposable>().AutoActivate();
        }

        private static ContainerRegistrationAssertions GetSut(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().ShouldHave();
        }

        // ReSharper disable ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Dummy : IDisposable { public void Dispose() { } }

        [ExcludeFromCodeCoverage]
        private class NamedDummy : ICloneable { public object Clone() { return this; } }

        [ExcludeFromCodeCoverage]
        private class KeyedDummy : IComparable { public int CompareTo(object obj) { return 42; } }

    }
}