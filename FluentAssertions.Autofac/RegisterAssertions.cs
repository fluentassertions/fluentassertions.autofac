using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace FluentAssertions.Autofac;

/// <summary>
///     Contains a number of methods to assert that an <see cref="IComponentContext" /> is in the expected state.
/// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
public class RegisterAssertions : RegistrationAssertions
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RegisterAssertions" /> class.
    /// </summary>
    /// <param name="subject">The container</param>
    /// <param name="type">The type that should be registered on the container</param>
    public RegisterAssertions(IComponentContext subject, Type type) : base(subject, type)
    {
    }

    /// <summary>
    ///     Asserts that the specified implementation type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    /// <typeparam name="TResolve">The type to resolve</typeparam>
    public RegisterAssertions As<TResolve>()
    {
        var instances = Subject.Resolve<IEnumerable<TResolve>>().ToArray();
        var resolved = instances.FirstOrDefault(instance => instance.GetType() == Type);
        resolved.Should().NotBeNull($"Type '{Type}' should be registered as '{typeof(TResolve)}'");
        return this;
    }

    /// <summary>
    ///     Asserts that the specified implementation type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    /// <param name="type">The type to resolve</param>
    /// <param name="types">Optional types to resolve</param>
    public RegisterAssertions As(Type type, params Type[] types)
    {
        AssertResolveAs(type);
        types?.ToList().ForEach(AssertResolveAs);
        return this;
    }

    /// <summary>
    ///     Asserts that the registered service type can be resolved from the current <see cref="IComponentContext" />.
    /// </summary>
    public RegisterAssertions AsSelf()
    {
        return As(Type);
    }

    /// <summary>
    ///     Asserts that all implemented interfaces of the registered service type can be resolved from the current
    ///     <see cref="IComponentContext" />.
    /// </summary>
    public RegisterAssertions AsImplementedInterfaces()
    {
        GetImplementedInterfaces(Type).ForEach(AssertResolveAs);
        return this;
    }

    private void AssertResolveAs(Type serviceType)
    {
        new ResolveAssertions(Subject, serviceType).As(Type);
    }

    private static List<Type> GetImplementedInterfaces(Type type)
    {
        var interfaces = type.GetTypeInfo().ImplementedInterfaces
            .Where(i => i != typeof(IDisposable))
            .ToList();
        if (type.GetTypeInfo().IsInterface)
            interfaces.Add(type);
        return interfaces;
    }
}
