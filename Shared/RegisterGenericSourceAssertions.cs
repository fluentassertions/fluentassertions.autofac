using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="IContainer" /> is in the expected state.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class RegisterGenericSourceAssertions : ReferenceTypeAssertions<IContainer, RegisterGenericSourceAssertions>
    {
        private readonly Type _genericComponentType;
        private readonly IEnumerable<IRegistrationSource> _sources;

        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
#if !PORTABLE && !CORE_CLR
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        protected override string Context => nameof(IContainer);

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterGenericSourceAssertions" /> class.
        /// </summary>
        /// <param name="subject">The container</param>
        /// <param name="genericComponentType">The type that should be registered on the container</param>
        public RegisterGenericSourceAssertions(IContainer subject, Type genericComponentType)
        {
            if (genericComponentType == null) throw new ArgumentNullException(nameof(genericComponentType));
            if (!genericComponentType.IsGenericType) throw new ArgumentException("Component type must be generic", nameof(genericComponentType));

            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            _sources = subject.ComponentRegistry.Sources;
            _genericComponentType = genericComponentType;
        }

        /// <summary>
        ///   Asserts that the specified implementation type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        /// <param name="genericServiceType">The type to resolve</param>
        public RegistrationAssertions As(Type genericServiceType)
        {
            if (genericServiceType == null) throw new ArgumentNullException(nameof(genericServiceType));
            if (!genericServiceType.IsGenericType) throw new ArgumentException("Service type must be generic", nameof(genericServiceType));

            if (!Implements(_genericComponentType, genericServiceType))
            {
                throw new ArgumentException(
                    $"Generic component must implement specified generic service. Component={_genericComponentType.FullName} Service={genericServiceType.FullName}");
            }

            var registration = ResolveRegistrationFromRegistrationSources(genericServiceType);
            registration.Should()
                .NotBeNull(
                    $"It must be a registration source for generic service {genericServiceType.FullName}");

            var registrationAssertions = new RegistrationAssertions(Subject, registration);

            var componentType = _genericComponentType.MakeGenericType(typeof(object));
            registrationAssertions.Type.Should()
                .Be(componentType, "Registration type ...");

            return registrationAssertions;
        }

        private IComponentRegistration ResolveRegistrationFromRegistrationSources(Type genericServiceType)
        {
            var serviceType = genericServiceType.MakeGenericType(typeof(object));
            var typedService = new TypedService(serviceType);

            foreach (var registrationSource in _sources)
            {
                var registration = registrationSource
                    .RegistrationsFor(typedService, Subject.ComponentRegistry.RegistrationsFor)
                    .FirstOrDefault();

                if (registration != null)
                {
                    return registration;
                }     
            }

            return null;
        }

        private bool Implements(Type type, Type contract)
        {
            // Ported from fasterflect https://github.com/buunguyen/fasterflect/blob/d4b15d15a29a9bfe46e620ba7eacd90621f8b146/Fasterflect/Fasterflect/Extensions/Core/TypeExtensions.cs#L54
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (contract == null) throw new ArgumentNullException(nameof(contract));
            if (!contract.IsInterface) throw new ArgumentException($"Contract type must be an interface. {nameof(contract)}={contract.FullName}", nameof(contract));

            if (type == contract)
            {
                return false;
            }

            if (contract.IsGenericTypeDefinition &&
                type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .Select(t => t.GetGenericTypeDefinition())
                    .Any(gt => gt == contract))
            {
                return true;
            }

            return contract.IsAssignableFrom(type);
        }
    }
}
