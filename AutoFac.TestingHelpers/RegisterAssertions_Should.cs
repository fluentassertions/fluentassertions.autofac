using System;
using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    [TestFixtureFor(typeof(RegisterAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegisterAssertions_Should
    {

        [Test]
        public void Register_As()
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
            var container = Configure(builder => 
                builder.RegisterType<Dummy>().SingleInstance()
                );

            container.Should().RegisterType<Dummy>()
                .SingleInstance();

            // TODO: other lifetimes
        }

        private static IContainer Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build();
        }

        // ReSharper disable ClassNeverInstantiated.Local
        private class Dummy : IDisposable
        {
            public void Dispose() { }
        }

        private class NamedDummy : ICloneable
        {
            public object Clone() { return this; }
        }

        private class KeyedDummy : IComparable
        {
            public int CompareTo(object obj) { return 42; }
        }
    }
}