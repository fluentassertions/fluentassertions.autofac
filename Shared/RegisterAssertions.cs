using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="IContainer" /> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class RegisterAssertions : RegistrationAssertions
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterAssertions" /> class.
        /// </summary>
        /// <param name="subject">The container</param>
        /// <param name="type">The type that should be registered on the container</param>
        public RegisterAssertions(IContainer subject, Type type) : base(subject, type)
        {
        }

        /// <summary>
        ///   Asserts that the specified implementation type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        /// <typeparam name="TResolve">The type to resolve</typeparam>
        public RegisterAssertions As<TResolve>()
        {
            var instances = Subject.Resolve<IEnumerable<TResolve>>().ToArray();
            var resolved = instances.FirstOrDefault(instance => instance.GetType() == Type);
            resolved.Should().NotBeNull($"Type '{Type}' should be registered as '{typeof (TResolve)}'");
            return this;
        }

        /// <summary>
        ///   Asserts that the specified implementation type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        /// <param name="type">The type to resolve</param>
        public RegisterAssertions As(Type type)
        {
            var actual = Subject.Resolve(type);
            actual.Should().BeOfType(Type, $"Type '{Type}' should be registered as '{type}'");
            return this;
        }

        /// <summary>
        ///   Asserts that the registered service type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        public RegisterAssertions AsSelf()
        {
            return As(Type);
        }
    }
}