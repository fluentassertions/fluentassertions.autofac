using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    /// <inheritdoc />
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="T:Autofac.IComponentContext" /> is in the expected state.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class RegisterGenericSourceAssertions : ReferenceTypeAssertions<IComponentContext, RegisterGenericSourceAssertions>
    {
        private readonly Type _genericComponentTypeDefinition;

        /// <inheritdoc />
        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected override string Identifier => nameof(IComponentContext);

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterGenericSourceAssertions" /> class.
        /// </summary>
        /// <param name="subject">The container</param>
        /// <param name="genericComponentTypeDefinition">The type that should be registered on the container</param>
        public RegisterGenericSourceAssertions(IComponentContext subject, Type genericComponentTypeDefinition) : base(subject)
        {
            AssertGenericType(genericComponentTypeDefinition);
            _genericComponentTypeDefinition = genericComponentTypeDefinition;
        }

        /// <summary>
        ///     Asserts that the specified service type can be resolved from the current <see cref="IComponentContext" />.
        /// </summary>
        /// <param name="genericServiceTypeDefinition">The type to resolve</param>
        public RegistrationAssertions As(Type genericServiceTypeDefinition)
        {
            AssertGenericType(genericServiceTypeDefinition);
            var componentServicePairText =
                $"Component={_genericComponentTypeDefinition.FullName} Service={genericServiceTypeDefinition.FullName}";

            _genericComponentTypeDefinition.GetGenericArguments().Length.Should()
                .Be(genericServiceTypeDefinition.GetGenericArguments().Length,
                    $"the generic arguments count of both generic component and generic service must be equal. {componentServicePairText}.");

            var argumentTypes = Enumerable.Repeat(typeof(object),
                _genericComponentTypeDefinition.GetGenericArguments().Length).ToArray();
            var componentType = _genericComponentTypeDefinition.MakeGenericType(argumentTypes);
            var serviceType = genericServiceTypeDefinition.MakeGenericType(argumentTypes);

            componentType.Should()
                .Implement(serviceType,
                    $"component must implement specified service. {componentServicePairText}.");

            var registration = GetRegistrationFromSources(serviceType);
            registration.Should()
                .NotBeNull(
                    $"it must be a registration source providing registrations for service {genericServiceTypeDefinition.FullName}");

            registration.Activator.LimitType.Should()
                .Be(componentType,
                    $"the generic component type definition in the registration must be {_genericComponentTypeDefinition.FullName}.");

            return new RegistrationAssertions(Subject, registration);
        }

        private IComponentRegistration GetRegistrationFromSources(Type serviceType)
        {
            var typedService = new TypedService(serviceType);

            foreach (var registrationSource in Subject.ComponentRegistry.Sources)
            {
                var registration = registrationSource
                    .RegistrationsFor(typedService, Accessor)
                    .FirstOrDefault();

                if (registration != null)
                {
                    return registration;
                }
            }

            return null;
        }

        private IEnumerable<ServiceRegistration> Accessor(Service service)
        {
            return Subject.ComponentRegistry.RegistrationsFor(service)
                .Select(c => new ServiceRegistration(ServicePipelines.DefaultServicePipeline, c));
        }

        private static void AssertGenericType(Type genericTypeDefinition)
        {
            if (genericTypeDefinition == null)
            {
                throw new ArgumentNullException(nameof(genericTypeDefinition));
            }

            if (!genericTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Type must be a generic type definition.",
                    nameof(genericTypeDefinition));
            }
        }
    }
}
