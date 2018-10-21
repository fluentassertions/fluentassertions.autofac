using System;
using Autofac;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class RegisterAssertions_Should
    {
        [Fact]
        public void Register_Type_As()
        {
            var containerShouldHave = Configure(builder =>
                builder.RegisterType<Dummy>()
                    .AsSelf()
                    .As<IDisposable>()
                    .AsImplementedInterfaces()
                );

            AssertAsRegistrations(containerShouldHave.Registered<Dummy>());
        }

        [Fact]
        public void Register_Instance()
        {
            var instance = new Dummy();

            var containerShouldHave = Configure(builder =>
                builder.RegisterInstance(instance)
                    .AsSelf()
                    .As<IDisposable>()
                    .AsImplementedInterfaces());

            AssertAsRegistrations(containerShouldHave.Registered(instance));
        }

        private static ContainerRegistrationAssertions Configure(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should().Have();
        }

        private static void AssertAsRegistrations(RegisterAssertions dummyShouldBeRegistered)
        {
            dummyShouldBeRegistered
                .AsSelf()
                .As<IDisposable>()
                .As(typeof(IDisposable))
                .As(typeof(IDisposable), typeof(Dummy))
                .AsImplementedInterfaces(); 
        }

#if !NETSTANDARD_1X
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        // ReSharper disable ClassNeverInstantiated.Local
        private class Dummy : IDisposable { public void Dispose() { } }
    }
}
