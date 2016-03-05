using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof (ContainerRegistrationAssertions))]
    // ReSharper disable InconsistentNaming
    internal class ContainerRegistrationAssertions_Should
    {
        [Test]
        public void RegisterType()
        {
            var sut = Configure(builder => builder.RegisterType<Dummy>()).Should().Have();
            sut.Registered<Dummy>();
            sut.Registered(typeof(Dummy));
        }

        [Test]
        public void RegisterInstance()
        {
            var instance = new Dummy();
            var container = Configure(builder => builder.RegisterInstance(instance));
            container.Should().Have().Registered(instance);
        }

        [Test]
        public void NotRegister()
        {
            var sut = Configure().Should().Have();
            sut.NotRegistered<IDisposable>();
            sut.NotRegistered(typeof(IDisposable));
            sut.NotRegistered<IDisposable>("foo");
            sut.NotRegistered("bar", typeof(IDisposable));
            sut.NotRegistered<IDisposable>(42);
            sut.NotRegistered(42, typeof(IDisposable));
        }

        private static IContainer Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build();
        }

        [ExcludeFromCodeCoverage]
        // ReSharper disable ClassNeverInstantiated.Local
        private class Dummy : IDisposable
        {
            public void Dispose() { }
        }
    }
}