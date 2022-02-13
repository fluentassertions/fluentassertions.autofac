using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac;

/// <inheritdoc />
/// <summary>
///     Contains a number of methods to assert that an <see cref="T:Autofac.IComponentContext" /> has registered expected
///     services.
/// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
public class
    ContainerRegistrationAssertions : ReferenceTypeAssertions<IComponentContext, ContainerRegistrationAssertions>
{
    /// <inheritdoc />
    /// <summary>
    ///     Returns the type of the subject the assertion applies on.
    /// </summary>
    [ExcludeFromCodeCoverage]
    protected override string Identifier => nameof(IComponentContext);

    /// <summary>
    ///     Initializes a new instance of the <see cref="ContainerRegistrationAssertions" /> class.
    /// </summary>
    /// <param name="subject">The subject</param>
    public ContainerRegistrationAssertions(IComponentContext subject) : base(subject)
    {
    }

    /// <summary>
    ///     Returns an <see cref="RegisterAssertions" /> object that can be used to assert the current
    ///     <see cref="IComponentContext" /> and <see typeparamref="TService" />.
    /// </summary>
    public RegisterAssertions Registered<TService>()
    {
        return new RegisterAssertions(Subject, typeof(TService));
    }

    /// <summary>
    ///     Returns an <see cref="RegisterAssertions" /> object that can be used to assert the current
    ///     <see cref="IComponentContext" /> and the specified type.
    /// </summary>
    public RegisterAssertions Registered(Type type)
    {
        return new RegisterAssertions(Subject, type);
    }

    /// <summary>
    ///     Returns an <see cref="RegisterAssertions" /> object that can be used to assert the current
    ///     <see cref="IComponentContext" /> and the specified instance.
    /// </summary>
    public RegisterAssertions Registered(object instance)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        return new RegisterAssertions(Subject, instance.GetType());
    }

    /// <summary>
    ///     Asserts that the specified <see typeparamref="TService" /> has not been registered on the current
    ///     <see cref="IComponentContext" />.
    /// </summary>
    /// <typeparam name="TService">The service type</typeparam>
    public void NotRegistered<TService>()
    {
        NotRegistered(typeof(TService));
    }

    /// <summary>
    ///     Asserts that the specified service type has not been registered on the current <see cref="IComponentContext" />.
    /// </summary>
    /// <param name="type">The service type</param>
    public void NotRegistered(Type type)
    {
        Subject.IsRegistered(type).Should().BeFalse($"Type '{type}' should not be registered");
    }

    /// <summary>
    ///     Asserts that the specified <see typeparamref="TService" /> has not been registered on the current
    ///     <see cref="IComponentContext" /> with the specified name.
    /// </summary>
    /// <param name="serviceName">The service name</param>
    /// <typeparam name="TService">The service type</typeparam>
    public void NotRegistered<TService>(string serviceName)
    {
        NotRegistered(serviceName, typeof(TService));
    }

    /// <summary>
    ///     Asserts that the specified service type has not been registered on the current <see cref="IComponentContext" />
    ///     with the
    ///     specified name.
    /// </summary>
    /// <param name="serviceName">The service name</param>
    /// <param name="type">The service type</param>
    public void NotRegistered(string serviceName, Type type)
    {
        Subject.IsRegisteredWithName(serviceName, type).Should()
            .BeFalse($"Type '{type}' should not be registered with name '{serviceName}'");
    }

    /// <summary>
    ///     Asserts that the specified service type has not been registered on the current <see cref="IComponentContext" />
    ///     with the
    ///     specified key.
    /// </summary>
    /// <param name="serviceKey">The service key</param>
    /// <typeparam name="TService">The service type</typeparam>
    public void NotRegistered<TService>(object serviceKey)
    {
        NotRegistered(serviceKey, typeof(TService));
    }

    /// <summary>
    ///     Asserts that the specified service type has not been registered on the current <see cref="IComponentContext" />
    ///     with the
    ///     specified key.
    /// </summary>
    /// <param name="serviceKey">The service key</param>
    /// <param name="type">The service type</param>
    public void NotRegistered(object serviceKey, Type type)
    {
        Subject.IsRegisteredWithKey(serviceKey, type).Should()
            .BeFalse($"Type '{type}' should not be registered with key '{serviceKey}'");
    }

    /// <summary>
    ///     Returns an <see cref="RegisterGenericSourceAssertions" /> object that can be used to assert the current
    ///     <see cref="IComponentContext" /> and the specified generic type.
    /// </summary>
    public RegisterGenericSourceAssertions RegisteredGeneric(Type genericComponentTypeDefinition)
    {
        return new RegisterGenericSourceAssertions(Subject, genericComponentTypeDefinition);
    }
}
