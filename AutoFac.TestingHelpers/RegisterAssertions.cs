using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac.Core.Lifetime;
using FluentAssertions;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    public class RegisterAssertions
    {
        private readonly Type _type;
        private Type _serviceType;
        protected readonly IContainer Container;

        public RegisterAssertions(IContainer container, Type type) 
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            Container = container;
            _type = type;
            _serviceType = type;
        }

        public RegisterAssertions As<TResolve>()
        {
            _serviceType = typeof (TResolve);
            var instances = Container.Resolve<IEnumerable<TResolve>>().ToArray();
            var resolved = instances.FirstOrDefault(instance => instance.GetType() == _type);
            resolved.Should().NotBeNull($"Type '{_type}' should be registered as '{typeof(TResolve)}'");
            return this;
        }

        public RegisterAssertions As(Type type, string because = null)
        {
            _serviceType = type;
            var actual = Container.Resolve(type);
            actual.Should().BeOfType(_type, because ?? $"Type '{_type}' should be registered as '{type}'");
            return this;
        }

        public RegisterAssertions AsSelf(string because = null)
        {
            _serviceType = _type;
            return As(_type, because);
        }

        public RegisterAssertions Named<TService>(string name, string because = null)
        {
            return Named(name, typeof(TService), because);
        }

        public RegisterAssertions Named(string name, Type type, string because = null)
        {
            Container.IsRegisteredWithName(name, type)
                .Should().BeTrue(because ?? $"Type '{type}' should be registered with name '{name}'");
            return this;
        }

        public RegisterAssertions Keyed<TService>(object key, string because = null)
        {
            return Keyed(key, typeof(TService), because);
        }

        public RegisterAssertions Keyed(object key, Type type, string because = null)
        {
            Container.IsRegisteredWithKey(key, type)
                .Should().BeTrue(because ?? $"Type '{type}' should be registered with key '{key}'");
            return this;
        }

        public RegisterAssertions SingleInstance(string because = null)
        {
            var cr = GetRegistration();
            AssertLifetime<RootScopeLifetime>(cr, because);
            cr.Sharing.Should().Be(InstanceSharing.Shared);
            return this;
        }

        public RegisterAssertions InstancePerDependency(string because = null)
        {
            var cr = GetRegistration();
            AssertLifetime<CurrentScopeLifetime>(cr, because);
            cr.Sharing.Should().Be(InstanceSharing.None);
            return this;
        }

        private void AssertLifetime<TLifetime>(IComponentRegistration cr, string because = null)
            where TLifetime : IComponentLifetime
        {
            cr.Lifetime.Should()
                .BeOfType<TLifetime>(because ??
                $"Type '{_type}' should be registered with lifetime '{typeof(TLifetime)}'");
        }

        private IComponentRegistration GetRegistration()
        {
            IComponentRegistration r;
            Assert.True(Container.ComponentRegistry.TryGetRegistration(new TypedService(_serviceType), out r));
            return r;
        }
    }
}