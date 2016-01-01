using System;
using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace AutoFac.TestingHelpers
{
    [TestFixtureFor(typeof(RegisterAssertions<>))]
    // ReSharper disable InconsistentNaming
    internal class RegisterAssertions_Should
    {
        private readonly IContainer _container = Configure();

        private static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dummy>().As<IDisposable>();
            return builder.Build();
        }

        [Test]
        public void Register_As()
        {
            var sut = _container.Should().RegisterType<Dummy>();
            sut.As<IDisposable>();
            sut.As(typeof(IDisposable));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Dummy : IDisposable
        {
            public void Dispose(){}
        }
    }
}