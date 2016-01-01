using System;
using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    [TestFixtureFor(typeof (ContainerAssertions))]
    // ReSharper disable InconsistentNaming
    internal class ContainerAssertions_Should
    {
        private readonly IContainer _container = new ContainerBuilder().Build();

        [Test]
        public void NotRegister()
        {
            _container.Should().NotRegister<IDisposable>();
            _container.Should().NotRegister(typeof(IDisposable));
            _container.Should().NotRegisterNamed<IDisposable>("foo");
            _container.Should().NotRegisterNamed("bar", typeof(IDisposable));
        }
    }
}