using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof (RegisterAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegisterAssertions_Should
    {

        [Test]
        public void Register_Type_As()
        {
            var containerShouldHave = Configure(builder =>
                builder.RegisterType<Dummy>()
                    .AsSelf()
                    .As<IDisposable>()
                );

            containerShouldHave.Registered<Dummy>()
                .AsSelf()
                .As<IDisposable>()
                .As(typeof (IDisposable));
        }

        [Test]
        public void Register_Instance()
        {
            var instance = new Dummy();

            var containerShouldHave = Configure(builder =>
                builder.RegisterInstance(instance)
                    .AsSelf()
                    .As<IDisposable>());

            containerShouldHave.Registered(instance)
                .AsSelf()
                .As<IDisposable>()
                .As(typeof (IDisposable));
        }

        private static ContainerRegistrationAssertions Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should().Have();
        }

        // ReSharper disable ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Dummy : IDisposable { public void Dispose() { } }

    }
}