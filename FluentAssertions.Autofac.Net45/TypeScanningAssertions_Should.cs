using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof (TypeScanningAssertions))]
    // ReSharper disable InconsistentNaming
    internal class TypeScanningAssertions_Should
    {
        [Test]
        public void Support_type_scanning()
        {
            var builder = new MockContainerBuilder();
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
        }

        // ReSharper disable ClassNeverInstantiated.Local
        private interface IDummy { }
        private class Dummy1 : IDummy { }
        private class Dummy2 : IDummy { }
        private class Dummy3 : IDummy { }
    }
}