using System.Linq;
using Autofac;
using Autofac.Core;
using Xunit;

namespace FluentAssertions.Autofac;

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
        registration.Activator.LimitType.Should().Be<string>();
        var service = (TypedService)registration.Services.Single();
        service.ServiceType.Should().Be<string>();

        var actual = container.Resolve<string>();
        actual.Should().Be(expected);
    }
}
