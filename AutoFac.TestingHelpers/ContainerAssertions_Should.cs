using System;
using System.Diagnostics.CodeAnalysis;
using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    [TestFixtureFor(typeof (ContainerAssertions))]
    // ReSharper disable InconsistentNaming
    internal class ContainerAssertions_Should
    {
        [Test]
        public void RegisterType()
        {
            var sut = Configure(builder => builder.RegisterType<Dummy>()).Should();
            sut.RegisterType<Dummy>();
            sut.RegisterType(typeof(Dummy));
        }

        [Test]
        public void RegisterInstance()
        {
            var instance = new Dummy();
            var sut = Configure(builder => builder.RegisterInstance(instance)).Should();
            sut.RegisterInstance(instance);
        }

        [Test]
        public void NotRegister()
        {
            var sut = Configure().Should();
            sut.NotRegister<IDisposable>();
            sut.NotRegister(typeof(IDisposable));
            sut.NotRegisterNamed<IDisposable>("foo");
            sut.NotRegisterNamed("bar", typeof(IDisposable));
            sut.NotRegisterKeyed<IDisposable>(42);
            sut.NotRegisterKeyed(42, typeof(IDisposable));
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