using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;

namespace FluentAssertions.Autofac
{
    public class RegistrationAssertions
    {
        protected IContainer Container;
        protected Type Type;
        private readonly IComponentRegistration _registration;

        public RegistrationAssertions(IContainer container, Type type)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            if (type == null) throw new ArgumentNullException(nameof(type));
            Container = container;
            Type = type;
            _registration = GetRegistration();
        }

        public RegistrationAssertions Named<TService>(string name)
        {
            return Named(name, typeof (TService));
        }

        public RegistrationAssertions Named(string name, Type type)
        {
            Container.IsRegisteredWithName(name, type)
                .Should().BeTrue($"Type '{type}' should be registered with name '{name}'");
            return this;
        }

        public RegistrationAssertions Keyed<TService>(object key)
        {
            return Keyed(key, typeof (TService));
        }

        public RegistrationAssertions Keyed(object key, Type type)
        {
            Container.IsRegisteredWithKey(key, type)
                .Should().BeTrue($"Type '{type}' should be registered with key '{key}'");
            return this;
        }

        public RegistrationAssertions SingleInstance()
        {
            return Lifetime<RootScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegistrationAssertions InstancePerDependency()
        {
            return Lifetime<CurrentScopeLifetime>()
                .Shared(InstanceSharing.None);
        }

        public RegistrationAssertions InstancePerLifetimeScope()
        {
            return Lifetime<CurrentScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegistrationAssertions InstancePerMatchingLifetimeScope()
        {
            return Lifetime<MatchingScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegistrationAssertions InstancePerRequest()
        {
            return Lifetime<MatchingScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        public RegistrationAssertions InstancePerOwned<TService>()
        {
            return InstancePerOwned(typeof (TService));
        }

        public RegistrationAssertions InstancePerOwned(Type serviceType)
        {
            // TODO
            return InstancePerMatchingLifetimeScope();
        }

        public RegistrationAssertions ExternallyOwned()
        {
            return Owned(InstanceOwnership.ExternallyOwned);
        }

        public RegistrationAssertions OwnedByLifetimeScope()
        {
            return Owned(InstanceOwnership.OwnedByLifetimeScope);
        }

        public RegistrationAssertions Lifetime<TLifetime>(Action<TLifetime> assert = null)
            where TLifetime : IComponentLifetime
        {
            _registration.Lifetime.Should()
                .BeOfType<TLifetime>($"Type '{Type}' should be registered with lifetime '{typeof (TLifetime)}'");
            assert?.Invoke((TLifetime) _registration.Lifetime);
            return this;
        }

        public RegistrationAssertions Shared(InstanceSharing sharing)
        {
            _registration.Sharing.Should().Be(sharing, $"Type '{Type}' should be shared as '{sharing}'");
            return this;
        }

        public RegistrationAssertions Owned(InstanceOwnership ownership)
        {
            _registration.Ownership.Should().Be(ownership, $"Type '{Type}' should be owned '{ownership}'");
            return this;
        }

        public RegistrationAssertions AutoActivate()
        {
            _registration.Services.Should()
                .Contain(service => service.Description == "AutoActivate",
                    $"Type '{Type}' should be auto activated");

            return this;
        }

        private IComponentRegistration GetRegistration()
        {
            var registration = Container.ComponentRegistry.Registrations
                .FirstOrDefault(r => r.Activator.LimitType == Type);
            registration.Should().NotBeNull($"Type '{Type}' should be registered");
            return registration;
        }
    }
}