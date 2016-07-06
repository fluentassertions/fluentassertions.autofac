using System;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="IContainer" /> registers expected services.
    /// </summary>
    [DebuggerNonUserCode]
    public class RegistrationAssertions : ReferenceTypeAssertions<IContainer, RegistrationAssertions>
    {
        /// <summary>
        ///    The type that should be registered on the container
        /// </summary>
        protected readonly Type Type;
        private readonly IComponentRegistration _registration;

        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
#if !PORTABLE && !CORE_CLR
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        protected override string Context => nameof(IContainer);

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegistrationAssertions" /> class.
        /// </summary>
        /// <param name="subject">The container</param>
        /// <param name="type">The type that should be registered on the container</param>
        public RegistrationAssertions(IContainer subject, Type type)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (type == null) throw new ArgumentNullException(nameof(type));
            Subject = subject;
            Type = type;
            _registration = GetRegistration();
        }

        /// <summary>
        ///   Asserts that the specified <see typeparamref="TService"/> has been registered on the current <see cref="IContainer"/> with the specified name.
        /// </summary>
        /// <param name="name">The service name</param>
        /// <typeparam name="TService">The service type</typeparam>
        public RegistrationAssertions Named<TService>(string name)
        {
            return Named(name, typeof (TService));
        }

        /// <summary>
        ///   Asserts that the specified <see paramref="type"/> has been registered on the current <see cref="IContainer"/> with the specified name.
        /// </summary>
        /// <param name="name">The service name</param>
        /// <param name="type">The service type</param>
        public RegistrationAssertions Named(string name, Type type)
        {
            Subject.IsRegisteredWithName(name, type)
                .Should().BeTrue($"Type '{type}' should be registered with name '{name}'");
            return this;
        }

        /// <summary>
        ///   Asserts that the specified <see typeparamref="TService"/> has been registered on the current <see cref="IContainer"/> with the specified key.
        /// </summary>
        /// <param name="key">The service key</param>
        /// <typeparam name="TService">The service type</typeparam>
        public RegistrationAssertions Keyed<TService>(object key)
        {
            return Keyed(key, typeof (TService));
        }

        /// <summary>
        ///   Asserts that the specified <see typeparamref="TService"/> has been registered on the current <see cref="IContainer"/> with the specified key.
        /// </summary>
        /// <param name="key">The service key</param>
        /// <param name="type">The service type</param>
        public RegistrationAssertions Keyed(object key, Type type)
        {
            Subject.IsRegisteredWithKey(key, type)
                .Should().BeTrue($"Type '{type}' should be registered with key '{key}'");
            return this;
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as singleton on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions SingleInstance()
        {
            return Lifetime<RootScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'instance per dependency' on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions InstancePerDependency()
        {
            return Lifetime<CurrentScopeLifetime>()
                .Shared(InstanceSharing.None);
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'instance per lifetime scope' on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions InstancePerLifetimeScope()
        {
            return Lifetime<CurrentScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'instance per matching lifetime scope' on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions InstancePerMatchingLifetimeScope()
        {
            return Lifetime<MatchingScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'instance per request' on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions InstancePerRequest()
        {
            return Lifetime<MatchingScopeLifetime>()
                .Shared(InstanceSharing.Shared);
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'instance per owned' of the specified <see typeparamref="TService"/> on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions InstancePerOwned<TService>()
        {
            return InstancePerOwned(typeof (TService));
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'instance per owned' of the specified <see paramref="type"/> on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions InstancePerOwned(Type serviceType)
        {
            // TODO
            return InstancePerMatchingLifetimeScope();
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'externally owned' on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions ExternallyOwned()
        {
            return Owned(InstanceOwnership.ExternallyOwned);
        }

        /// <summary>
        ///   Asserts that the current service type has been registered as 'owned by lifetime scope' on the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions OwnedByLifetimeScope()
        {
            return Owned(InstanceOwnership.OwnedByLifetimeScope);
        }

        /// <summary>
        ///   Asserts the current service type has been registered using the specified <see typeparamref="TLifetime"/> on the current <see cref="IContainer"/>.
        /// </summary>
        /// <param name="assert">An optional custom assertion action to execute on the <typeparamref name="TLifetime"/></param>
        /// <typeparam name="TLifetime"></typeparam>
        public RegistrationAssertions Lifetime<TLifetime>(Action<TLifetime> assert = null)
            where TLifetime : IComponentLifetime
        {
            _registration.Lifetime.Should()
                .BeOfType<TLifetime>($"Type '{Type}' should be registered with lifetime '{typeof (TLifetime)}'");
            assert?.Invoke((TLifetime) _registration.Lifetime);
            return this;
        }

        /// <summary>
        ///   Asserts the current service type has been registered using the specified <see cref="InstanceSharing"/> on the current <see cref="IContainer"/>.
        /// </summary>
        /// <param name="sharing">The instance sharing mode</param>
        public RegistrationAssertions Shared(InstanceSharing sharing)
        {
            _registration.Sharing.Should().Be(sharing, $"Type '{Type}' should be shared as '{sharing}'");
            return this;
        }

        /// <summary>
        ///   Asserts the current service type has been registered using the specified <see cref="InstanceOwnership"/> on the current <see cref="IContainer"/>.
        /// </summary>
        /// <param name="ownership">The instance ownership mode</param>
        public RegistrationAssertions Owned(InstanceOwnership ownership)
        {
            _registration.Ownership.Should().Be(ownership, $"Type '{Type}' should be owned '{ownership}'");
            return this;
        }

        /// <summary>
        ///   Asserts the current service type has been registered with auto activation on the current <see cref="IContainer"/>.
        /// </summary>
        /// <returns></returns>
        public RegistrationAssertions AutoActivate()
        {
            _registration.Services.Should()
                .Contain(service => service.Description == "AutoActivate",
                    $"Type '{Type}' should be auto activated");

            return this;
        }

        private IComponentRegistration GetRegistration()
        {
            var registration = Subject.ComponentRegistry.Registrations
                .FirstOrDefault(r => r.Activator.LimitType == Type);
            registration.Should().NotBeNull($"Type '{Type}' should be registered");
            return registration;
        }
    }
}