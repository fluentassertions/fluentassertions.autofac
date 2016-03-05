using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof (ResolveAssertions<>))]
    // ReSharper disable InconsistentNaming
    internal class ResolveAssertions_Should
    {
        [Test]
        public void Resolve()
        {
            var container = Configure();
            container.Invoking(x => x.Should().Resolve<IDisposable>())
                .ShouldThrow<AssertionException>()
                .WithMessage($"Expected container to resolve '{typeof (IDisposable)}'.");

            var disposable = Substitute.For<IDisposable>();
            container = Configure(builder => builder.RegisterInstance(disposable));
            container.Should().Resolve<IDisposable>();
        }

        [Test]
        public void Resolve_As()
        {
            var containerShould = Configure(builder =>
                builder.RegisterType<Dummy>()
                    .AsSelf()
                    .As<IDisposable>()
                ).Should();

            containerShould.Resolve<Dummy>();
            containerShould.Resolve<Dummy>().AsSelf();
            containerShould.Resolve<IDisposable>().As<Dummy>();
            containerShould.Resolve<IDisposable>().As(typeof(Dummy));
        }

        private static IContainer Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build();
        }

        // ReSharper disable ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Dummy : IDisposable { public void Dispose() { } }
    }
}