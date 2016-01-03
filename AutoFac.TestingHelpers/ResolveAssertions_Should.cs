using System;
using System.Diagnostics.CodeAnalysis;
using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    [TestFixtureFor(typeof (ResolveAssertions<>))]
    // ReSharper disable InconsistentNaming
    internal class ResolveAssertions_Should
    {
        [Test]
        public void Resolve_As()
        {
            var containerShould = Configure(builder =>
                builder.RegisterType<Dummy>()
                    .AsSelf()
                    .As<IDisposable>()
                );

            containerShould.Resolve<Dummy>();
            containerShould.Resolve<Dummy>().AsSelf();
            containerShould.Resolve<IDisposable>().As<Dummy>();
            containerShould.Resolve<IDisposable>().As(typeof(Dummy));
        }

        private static ContainerAssertions Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should();
        }

        // ReSharper disable ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Dummy : IDisposable { public void Dispose() { } }
    }
}