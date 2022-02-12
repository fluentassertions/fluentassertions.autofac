using Autofac;
using Xunit;

namespace FluentAssertions.Autofac;

// ReSharper disable InconsistentNaming
public class TypeScanningAssertions_Should
{
    [Fact]
    public void Support_type_scanning()
    {
        var builder = new ContainerBuilder();
        builder.RegisterAssemblyTypes(typeof(IDummy).Assembly)
            .Where(t => t.IsAssignableTo<IDummy>())
            .Except<Dummy3>()
            .AsSelf()
            .AsImplementedInterfaces()
            .As<IDummy>()
            .As(t => t.GetInterfaces()[0]);

        var container = builder.Build();
        var types =
            container.Should().RegisterAssemblyTypes(typeof(IDummy).Assembly)
                .Where(t => t.IsAssignableTo<IDummy>())
                .Except<Dummy3>()
                .AsSelf()
                .AsImplementedInterfaces()
                .As<IDummy>()
                .As(t => t.GetInterfaces()[0])
                .Types;

        container.Should().RegisterTypes(types)
            .AsSelf()
            .AsImplementedInterfaces()
            .As<IDummy>()
            .As(t => t.GetInterfaces()[0]);
    } // ReSharper disable ClassNeverInstantiated.Local
    private interface IDummy
    {
    }

    // ReSharper disable UnusedType.Local
    private class Dummy1 : IDummy
    {
    }

    private class Dummy2 : IDummy
    {
    }
    // ReSharper restore UnusedType.Local

    private class Dummy3 : IDummy
    {
    }
}
