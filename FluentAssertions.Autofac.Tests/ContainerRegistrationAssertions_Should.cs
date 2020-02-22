using System;
using Autofac;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class ContainerRegistrationAssertions_Should
    {
        [Fact]
        public void RegisterType()
        {
            var sut = Configure(builder => builder.RegisterType<Dummy>()).Should().Have();
            sut.Registered<Dummy>();
            sut.Registered(typeof(Dummy));
        }

        [Fact]
        public void RegisterInstance()
        {
            var instance = new Dummy();
            var container = Configure(builder => builder.RegisterInstance(instance));
            container.Should().Have().Registered(instance);
        }

        [Fact]
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

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        // ReSharper disable ClassNeverInstantiated.Local
        private class Dummy : IDisposable
        {
            public void Dispose() { }
        }
    }
}
