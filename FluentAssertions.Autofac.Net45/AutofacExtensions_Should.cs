using System.Linq;
using Autofac;
using Autofac.Core;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof (AutofacExtensions))]
    // ReSharper disable InconsistentNaming
    internal class AutofacExtensions_Should
    {
        [Test]
        public void Get_Registrations()
        {
            var builder = new ContainerBuilder();
            const string expected = "hello";
            builder.RegisterInstance(expected);
            var container = builder.Build();

            var registration = container.ComponentRegistry.GetRegistration<string>();
            var actual = registration.Activator.ActivateInstance(container, Enumerable.Empty<Parameter>());

            actual.Should().Be(expected);
        }
    }
}