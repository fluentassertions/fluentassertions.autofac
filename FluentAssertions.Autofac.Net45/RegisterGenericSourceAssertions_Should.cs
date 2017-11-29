using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof(RegisterGenericSourceAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegisterGenericSourceAssertions_Should
    {
        [Test]
        public void Register_Generic()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            containerShouldHave
                .RegisteredGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .SingleInstance();
        }

        private static ContainerRegistrationAssertions GetSut(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should().Have();
        }

        private interface IRepository<TEntity> { }

        [ExcludeFromCodeCoverage]
        private class Repository<TEntity> : IRepository<TEntity> { }
    }
}
