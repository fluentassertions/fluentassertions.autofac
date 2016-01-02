using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac.Core.Lifetime;
using FluentAssertions;

namespace Autofac.TestingHelpers
{
    public class RegisterAssertions
    {
        private readonly IContainer _container;
        private readonly Type _type;
        private readonly IComponentRegistration _registration;

        public RegisterAssertions(IContainer container, Type type)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
            _type = type;
            _registration = GetRegistration();
        }

        public RegisterAssertions As<TResolve>()
        {
            var instances = _container.Resolve<IEnumerable<TResolve>>().ToArray();
            var resolved = instances.FirstOrDefault(instance => instance.GetType() == _type);
            resolved.Should().NotBeNull($"Type '{_type}' should be registered as '{typeof (TResolve)}'");
            return this;
        }

        public RegisterAssertions As(Type type)
        {
            var actual = _container.Resolve(type);
            actual.Should().BeOfType(_type, $"Type '{_type}' should be registered as '{type}'");
            return this;
        }

        public RegisterAssertions AsSelf()
        {
            return As(_type);
        }

        public RegisterAssertions Named<TService>(string name)
        {
            return Named(name, typeof (TService));
        }

        public RegisterAssertions Named(string name, Type type)
        {
            _container.IsRegisteredWithName(name, type)
                .Should().BeTrue($"Type '{type}' should be registered with name '{name}'");
            return this;
        }

        public RegisterAssertions Keyed<TService>(object key)
        {
            return Keyed(key, typeof (TService));
        }

        public RegisterAssertions Keyed(object key, Type type)
        {
            _container.IsRegisteredWithKey(key, type)
                .Should().BeTrue($"Type '{type}' should be registered with key '{key}'");
            return this;
        }

        public RegisterAssertions SingleInstance()
        {
            return Lifetime<RootScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegisterAssertions InstancePerDependency()
        {
            return Lifetime<CurrentScopeLifetime>()
                .Shared(InstanceSharing.None);
        }

        public RegisterAssertions InstancePerLifetimeScope()
        {
            return Lifetime<CurrentScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegisterAssertions InstancePerMatchingLifetimeScope()
        {
            return Lifetime<MatchingScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegisterAssertions InstancePerRequest()
        {
            return Lifetime<MatchingScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegisterAssertions InstancePerOwned<TService>()
        {
            return InstancePerOwned(typeof (TService));
        }

        public RegisterAssertions InstancePerOwned(Type serviceType)
        {
            // TODO
            return InstancePerMatchingLifetimeScope();
        }

        public RegisterAssertions ExternallyOwned()
        {
            return Owned(InstanceOwnership.ExternallyOwned);
        }

        public RegisterAssertions OwnedByLifetimeScope()
        {
            return Owned(InstanceOwnership.OwnedByLifetimeScope);
        }

        public RegisterAssertions Lifetime<TLifetime>(Action<TLifetime> assert = null)
            where TLifetime : IComponentLifetime
        {
            _registration.Lifetime.Should()
                .BeOfType<TLifetime>($"Type '{_type}' should be registered with lifetime '{typeof (TLifetime)}'");
            assert?.Invoke((TLifetime) _registration.Lifetime);
            return this;
        }

        public RegisterAssertions Shared(InstanceSharing sharing)
        {
            _registration.Sharing.Should().Be(sharing, $"Type '{_type}' should be shared as '{sharing}'");
            return this;
        }

        public RegisterAssertions Owned(InstanceOwnership ownership)
        {
            _registration.Ownership.Should().Be(ownership, $"Type '{_type}' should be owned '{ownership}'");
            return this;
        }

        public RegisterAssertions AutoActivate()
        {
            _registration.Services.Should()
                .Contain(service => service.Description == "AutoActivate",
                 $"Type '{_type}' should be auto activated");

            return this;
        }

        private IComponentRegistration GetRegistration()
        {
            var registration = _container.ComponentRegistry.Registrations
                .FirstOrDefault(r => r.Activator.LimitType == _type);
            registration.Should().NotBeNull($"Type '{_type}' should be registered");
            return registration;
        }
    }
}