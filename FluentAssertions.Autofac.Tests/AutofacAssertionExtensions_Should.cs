using System;
using Autofac;
using NSubstitute;
using Xunit;

namespace FluentAssertions.Autofac;

// ReSharper disable InconsistentNaming
public class AutofacAssertionExtensions_Should
{
    [Fact]
    public void Provide_builder_extension()
    {
        var builder = new ContainerBuilder();
        builder.Should().ShouldBeOfType<BuilderAssertions>();
    }

    [Fact]
    public void Provide_register_extension()
    {
        var container = new ContainerBuilder().Build();
        container.Should().Have().ShouldBeOfType<ContainerRegistrationAssertions>();
    }

    [Fact]
    public void Provide_register_extension_on_lifetime_scope()
    {
        var scope = new ContainerBuilder().Build().BeginLifetimeScope();
        scope.Should().Have().ShouldBeOfType<ContainerRegistrationAssertions>();
    }

    [Fact]
    public void Provide_resolve_extension()
    {
        var builder = new ContainerBuilder();
        builder.RegisterInstance(Substitute.For<IDisposable>());
        var container = builder.Build();
        container.Should().Resolve<IDisposable>().ShouldBeOfType<ResolveAssertions>();
    }
}
