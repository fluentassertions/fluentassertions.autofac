using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac;

/// <inheritdoc />
/// <summary>
///     Contains a number of methods to assert that an <see cref="T:Autofac.IComponentContext" /> is in the expected state.
/// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
public class
    RegisterGenericSourceAssertions : ReferenceTypeAssertions<IComponentContext, RegisterGenericSourceAssertions>
{
    private readonly Type _type;

    /// <inheritdoc />
    /// <summary>
    ///     Returns the type of the subject the assertion applies on.
    /// </summary>
    [ExcludeFromCodeCoverage]
    protected override string Identifier => nameof(IComponentContext);

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegisterGenericSourceAssertions" /> class.
    /// </summary>
    /// <param name="subject">The component context</param>
    /// <param name="type">The type that should be registered on the container</param>
    public RegisterGenericSourceAssertions(IComponentContext subject, Type type) :
        base(subject)
    {
        AssertGenericType(type);
        _type = type;
    }

    /// <summary>
    ///     Asserts that the specified service type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    /// <param name="type">The type to resolve</param>
    public RegistrationAssertions As(Type type)
    {
        var serviceType = GenericServiceTypeFor(type, out var componentType);
        var service = new TypedService(serviceType);
        var registration = RegistrationFor(type, service, componentType);
        return new RegistrationAssertions(Subject, registration);
    }

    /// <summary>
    ///     Asserts that the specified service type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    /// <param name="serviceName">The service name</param>
    /// <param name="type">The type to resolve</param>
    public RegistrationAssertions Named(string serviceName, Type type)
    {
        var serviceType = GenericServiceTypeFor(type, out var componentType);
        var service = new KeyedService(serviceName, serviceType);
        var registration = RegistrationFor(type, service, componentType);
        return new RegistrationAssertions(Subject, registration);
    }

    private IComponentRegistration RegistrationFor(Type type, Service service, Type componentType)
    {
        var registration = RegistrationsFor(service).FirstOrDefault();
        registration.Should()
            .NotBeNull($"there should be a registration source providing registrations for service {type.FullName}");
        registration?.Activator.LimitType.Should()
            .Be(componentType, $"the generic component type definition registered should be {_type.FullName}.");
        return registration;
    }

    private Type GenericServiceTypeFor(Type type, out Type componentType)
    {
        AssertGenericType(type);
        var componentServicePairText =
            $"Component={_type.FullName} Service={type.FullName}";

        _type.GetGenericArguments().Should().HaveCount(type.GetGenericArguments().Length,
            $"the generic arguments count of both generic component and generic service must be equal. {componentServicePairText}.");

        var argumentTypes = Enumerable.Repeat(typeof(object),
            _type.GetGenericArguments().Length).ToArray();
        componentType = _type.MakeGenericType(argumentTypes);
        var serviceType = type.MakeGenericType(argumentTypes);

        componentType.Should().Implement(serviceType,
                $"component must implement specified service. {componentServicePairText}.");
        return serviceType;
    }

    private IEnumerable<IComponentRegistration> RegistrationsFor(Service service)
    {
        return Subject.ComponentRegistry.Sources
            .SelectMany(sources => sources.RegistrationsFor(service, Accessor)
                .ToList());
    }

    private IEnumerable<ServiceRegistration> Accessor(Service service)
    {
        return Subject.ComponentRegistry.RegistrationsFor(service)
            .Select(c => new ServiceRegistration(ServicePipelines.DefaultServicePipeline, c));
    }

    private static void AssertGenericType(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (!type.IsGenericTypeDefinition)
        {
            throw new ArgumentException("Type must be a generic type definition.",
                nameof(type));
        }
    }
}
