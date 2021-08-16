using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NSubstitute;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class ResolveAssertions_Should
    {
        [Fact]
        public void Resolve()
        {
            var container = Configure();
            container.Invoking(x => x.Should().Resolve<IDisposable>())
                .Should().Throw<Exception>()
                .WithMessage($"Expected container to resolve '{typeof(IDisposable)}' but it did not.");

            var disposable = Substitute.For<IDisposable>();
            container = Configure(builder => builder.RegisterInstance(disposable));
            container.Should().Resolve<IDisposable>();
        }

        [Fact]
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

        [Fact]
        public void Assert_AutoActivation()
        {
            var container = Configure(builder => builder.RegisterType<Dummy>().AsSelf().AutoActivate());
            container.Should().Resolve<Dummy>().AutoActivate();

            container = Configure(builder => builder.RegisterType<Dummy>().AutoActivate());
            container.Should().AutoActivate<Dummy>();
            container.Should().Invoking(x => x.Resolve<Dummy>()).Should()
                .Throw<Exception>("type not registered AS something");
        }

        private static IContainer Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build();
        }

        [ExcludeFromCodeCoverage]
        private class Dummy : IDisposable
        {
            public void Dispose() { }
        }
    }
}
