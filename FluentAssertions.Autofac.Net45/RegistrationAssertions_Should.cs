using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Core;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof(RegistrationAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegistrationAssertions_Should
    {
        [Test]
        public void Register_Named()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterType<NamedDummy>().Named<ICloneable>("Dummy")
                );

            containerShouldHave.Registered<NamedDummy>()
                .Named<ICloneable>("Dummy")
                .Keyed<ICloneable>("Dummy");
        }

        [Test]
        public void Register_Keyed()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterType<KeyedDummy>().Keyed<IComparable>(42)
                );

            containerShouldHave.Registered<KeyedDummy>()
                .Keyed<IComparable>(42);
        }

        [Test]
        public void Register_Lifetime()
        {
            var containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().SingleInstance());
            containerShouldHave.Registered<Dummy>().SingleInstance();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerDependency());
            containerShouldHave.Registered<Dummy>().InstancePerDependency();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerLifetimeScope());
            containerShouldHave .Registered<Dummy>().InstancePerLifetimeScope();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerMatchingLifetimeScope());
            containerShouldHave .Registered<Dummy>().InstancePerMatchingLifetimeScope();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerRequest());
            containerShouldHave .Registered<Dummy>().InstancePerRequest();

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().InstancePerOwned<ICloneable>());
            containerShouldHave .Registered<Dummy>().InstancePerOwned<ICloneable>();
        }

        [Test]
        public void Register_Ownership()
        {
            var containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().ExternallyOwned());
            containerShouldHave .Registered<Dummy>()
                .ExternallyOwned()
                .Owned(InstanceOwnership.ExternallyOwned);

            containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().OwnedByLifetimeScope());
            containerShouldHave .Registered<Dummy>()
                .OwnedByLifetimeScope()
                .Owned(InstanceOwnership.OwnedByLifetimeScope);
        }

        [Test]
        public void Register_AutoActivate()
        {
            var containerShouldHave = GetSut(builder => builder.RegisterType<Dummy>().As<IDisposable>().AutoActivate());
            containerShouldHave .Registered<Dummy>().As<IDisposable>().AutoActivate();
        }

        [Test]
        public void Register_parameters()
        {
            var builder = new ContainerBuilder();

            const string paramName = "name";
            const string paramValue = "Name";

            builder.RegisterType<Dummy>()
                .As<IDisposable>()
                .WithParameter(paramName, paramValue)
                .WithParameter(new NamedParameter(paramName, paramValue))
                .WithParameter(new PositionalParameter(0, paramValue));

            var container = builder.Build();
            container.Should().Have()
                .Registered<Dummy>()
                .As<IDisposable>()
                .WithParameter(paramName, paramValue)
                .WithParameter(new NamedParameter(paramName, paramValue))
                .WithParameter(new PositionalParameter(0, paramValue))
                ;
        }

        private static ContainerRegistrationAssertions GetSut(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should().Have();
        }

        // ReSharper disable ClassNeverInstantiated.Local
        [ExcludeFromCodeCoverage]
        private class Dummy : IDisposable { public void Dispose() { } }

        [ExcludeFromCodeCoverage]
        private class NamedDummy : ICloneable { public object Clone() { return this; } }

        [ExcludeFromCodeCoverage]
        private class KeyedDummy : IComparable { public int CompareTo(object obj) { return 42; } }

        [ExcludeFromCodeCoverage]
        private class ParameterizedDummy : Dummy
        {
            public string Name { get; }
            public ParameterizedDummy(string name) { Name = name; }
        }
    }
}