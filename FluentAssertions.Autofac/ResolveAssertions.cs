using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac;

/// <inheritdoc />
/// <summary>
///     Contains a number of methods to assert that expected services can actually be resolved from an
///     <see cref="T:Autofac.IComponentContext" />.
/// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
public class ResolveAssertions : ReferenceTypeAssertions<IComponentContext, ResolveAssertions>
{
    private readonly Type _serviceType;
    private readonly List<object> _instances = new();

    /// <inheritdoc />
    /// <summary>
    ///     Returns the type of the subject the assertion applies on.
    /// </summary>
    [ExcludeFromCodeCoverage]
    protected override string Identifier => nameof(IComponentContext);

    /// <summary>
    ///     Initializes a new instance of the <see cref="ResolveAssertions" /> class.
    /// </summary>
    /// <param name="container">The container</param>
    /// <param name="serviceType">The service type</param>
    public ResolveAssertions(IComponentContext container, Type serviceType) : base(container)
    {
        _serviceType = serviceType;
        var typeToResolve = typeof(IEnumerable<>).MakeGenericType(serviceType);
        if (Subject.Resolve(typeToResolve) is Array array)
        {
            _instances.AddRange(array.OfType<object>());
        }

        Execute.Assertion
            .ForCondition(_instances.Any())
            .FailWith($"Expected container to resolve '{_serviceType}' but it did not.");
    }

    /// <summary>
    ///     Asserts that the specified implementation type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    /// <typeparam name="TImplementation">The type to resolve</typeparam>
    // ReSharper disable once UnusedMember.Global
    public RegistrationAssertions As<TImplementation>()
    {
        return As(typeof(TImplementation));
    }

    /// <summary>
    ///     Asserts that the specified implementation type(s) can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    /// <param name="type">The type to resolve</param>
    /// <param name="types">Optional types to resolve</param>
    public RegistrationAssertions As(Type type, params Type[] types)
    {
        AssertTypeResolved(type);
        types.ToList().ForEach(AssertTypeResolved);
        return new RegistrationAssertions(Subject, type);
    }

    /// <summary>
    ///     Asserts that the registered service type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    public RegistrationAssertions AsSelf()
    {
        return As(_serviceType);
    }


    /// <summary>
    ///     Asserts that the service type has been registered with auto activation on the current
    ///     <see cref="IComponentContext" />.
    /// </summary>
    public void AutoActivate()
    {
        Subject.AssertAutoActivates(_serviceType);
    }

    private void AssertTypeResolved(Type type)
    {
        _instances.Should().Contain(instance => instance.GetType() == type,
            $"Type '{_serviceType}' should be resolved as '{type}'");
    }
}
