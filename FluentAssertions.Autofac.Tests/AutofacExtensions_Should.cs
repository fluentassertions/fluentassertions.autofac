using System.Linq;
using Autofac;
using Autofac.Core;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class AutofacExtensions_Should
    {
        [Fact]
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
