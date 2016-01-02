using System;
using System.Diagnostics.CodeAnalysis;
using Autofac.Core;
using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    [TestFixtureFor(typeof(RegisterAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegisterAssertions_Should
    {

        [Test]
        public void Register_Type_As()
        {
            var container = Configure(builder => 
                builder.RegisterType<Dummy>()
                    .AsSelf()
                    .As<IDisposable>()
                );

            container.Should().RegisterType<Dummy>()
                .AsSelf()
                .As<IDisposable>()
                .As(typeof(IDisposable));
        }

        [Test]
        public void Register_Instance()
        {
            var instance = new Dummy();

            var container = Configure(builder =>
                builder.RegisterInstance(instance)
                    .AsSelf()
                    .As<IDisposable>());

            container.Should().RegisterInstance(instance)
                .AsSelf()
                .As<IDisposable>()
                .As(typeof(IDisposable));
        }


        [Test]
        public void Register_Named()
        {
            var container = Configure(builder => 
                builder.RegisterType<NamedDummy>().Named<ICloneable>("Dummy")
                );

            container.Should().RegisterType<NamedDummy>()
                .Named<ICloneable>("Dummy")
                .Keyed<ICloneable>("Dummy");
        }

        [Test]
        public void Register_Keyed()
        {
            var container = Configure(builder =>
                builder.RegisterType<KeyedDummy>().Keyed<IComparable>(42)
                );

            container.Should().RegisterType<KeyedDummy>()
                .Keyed<IComparable>(42);
        }

        [Test]
        public void Register_Lifetime()
        {
            var container = Configure(builder => builder.RegisterType<Dummy>().SingleInstance());
            container.Should().RegisterType<Dummy>().SingleInstance();

            container = Configure(builder => builder.RegisterType<Dummy>().InstancePerDependency());
            container.Should().RegisterType<Dummy>().InstancePerDependency();

            container = Configure(builder => builder.RegisterType<Dummy>().InstancePerLifetimeScope());
            container.Should().RegisterType<Dummy>().InstancePerLifetimeScope();

            container = Configure(builder => builder.RegisterType<Dummy>().InstancePerMatchingLifetimeScope());
            container.Should().RegisterType<Dummy>().InstancePerMatchingLifetimeScope();

            container = Configure(builder => builder.RegisterType<Dummy>().InstancePerRequest());
            container.Should().RegisterType<Dummy>().InstancePerRequest();

            container = Configure(builder => builder.RegisterType<Dummy>().InstancePerOwned<ICloneable>());
            container.Should().RegisterType<Dummy>().InstancePerOwned<ICloneable>();
        }

        [Test]
        public void Register_Ownership()
        {
            var container = Configure(builder => builder.RegisterType<Dummy>().ExternallyOwned());
            container.Should().RegisterType<Dummy>()
                .ExternallyOwned()
                .Owned(InstanceOwnership.ExternallyOwned);

            container = Configure(builder => builder.RegisterType<Dummy>().OwnedByLifetimeScope());
            container.Should().RegisterType<Dummy>()
                .OwnedByLifetimeScope()
                .Owned(InstanceOwnership.OwnedByLifetimeScope);
        }

        [Test]
        public void Register_AutoActivate()
        {
            var container = Configure(builder => builder.RegisterType<Dummy>().As<IDisposable>().AutoActivate());
            container.Should().RegisterType<Dummy>().As<IDisposable>().AutoActivate();
        }

        private static IContainer Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build();
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