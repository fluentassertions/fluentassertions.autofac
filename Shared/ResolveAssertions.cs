using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains a number of methods to assert that expected services can actually be resolved from an <see cref="IContainer" />.
    /// </summary>
    [DebuggerNonUserCode]
    public class ResolveAssertions<TService> : ReferenceTypeAssertions<IContainer, ResolveAssertions<TService>>
    {
        private readonly List<TService> _instances;

        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
#if !PORTABLE && !CORE_CLR
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        protected override string Context => nameof(IContainer);
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResolveAssertions{TService}" /> class.
        /// </summary>
        /// <param name="container">The container</param>
        public ResolveAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            Subject = container;
            _instances = Subject.Resolve<IEnumerable<TService>>().ToList();

            Execute.Assertion
                .ForCondition(_instances.Any())
                .FailWith($"Expected container to resolve '{typeof(TService)}' but it did not.");
        }

        /// <summary>
        ///   Asserts that the specified implementation type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        /// <typeparam name="TImplementation">The type to resolve</typeparam>
        public RegistrationAssertions As<TImplementation>()
            where TImplementation : TService
        {
            return As(typeof (TImplementation));
        }

        /// <summary>
        ///   Asserts that the registered service type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        public RegistrationAssertions AsSelf()
        {
            return As<TService>();
        }

        /// <summary>
        ///   Asserts that the specified implementation type can be resolved from the current <see cref="IContainer"/>.
        /// </summary>
        /// <param name="type">The type to resolve</param>
        public RegistrationAssertions As(Type type)
        {
            _instances.Should().Contain(instance => instance.GetType() == type,
                $"Type '{typeof (TService)}' should be resolved as '{type}'");

            return new RegistrationAssertions(Subject, type);
        }
    }
}