using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Util;
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
    public class ContainerAssertions : ReferenceTypeAssertions<IComponentContext, ContainerAssertions>
    {
        /// <inheritdoc />
        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected override string Identifier => nameof(IComponentContext);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerAssertions" /> class.
        /// </summary>
        /// <param name="container">The subject</param>
        public ContainerAssertions(IComponentContext container) : base(container)
        {
        }

        /// <summary>
        ///     Returns an <see cref="ContainerRegistrationAssertions" /> object that can be used to assert the current
        ///     <see cref="IComponentContext" />.
        /// </summary>
        public ContainerRegistrationAssertions Have()
        {
            return new ContainerRegistrationAssertions(Subject);
        }

        /// <summary>
        ///     Returns an <see cref="ResolveAssertions" /> object that can be used to assert the current
        ///     <see paramref="serviceType" />.
        /// </summary>
        public ResolveAssertions Resolve(Type serviceType)
        {
            return new ResolveAssertions(Subject, serviceType);
        }

        /// <summary>
        ///     Returns an <see cref="ResolveAssertions" /> object that can be used to assert the current
        ///     <see typeparamref="TService" />.
        /// </summary>
        public ResolveAssertions Resolve<TService>()
        {
            return Resolve(typeof(TService));
        }

        /// <summary>
        ///     Asserts the specified type has been registered with auto activation on the current <see cref="IComponentContext" />.
        /// </summary>
        public void AutoActivate(Type type)
        {
            Subject.AssertAutoActivates(type);
        }

        /// <summary>
        ///     Asserts the specified type has been registered with auto activation on the current <see cref="IComponentContext" />.
        /// </summary>
        public void AutoActivate<TService>()
        {
            AutoActivate(typeof(TService));
        }

        /// <summary>
        ///     Returns an <see cref="TypeScanningAssertions" /> object that can be used to assert registered types on the current
        ///     <see cref="ContainerBuilder" />.
        /// </summary>
        /// <param name="assemblies"></param>
        public TypeScanningAssertions RegisterAssemblyTypes(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(assembly => assembly.GetLoadableTypes());
            return new TypeScanningAssertions(Subject, types);
        }

        /// <summary>
        ///     Returns an <see cref="TypeScanningAssertions" /> object that can be used to assert registered types on the current
        ///     <see cref="ContainerBuilder" />.
        /// </summary>
        /// <param name="types"></param>
        public TypeScanningAssertions RegisterTypes(IEnumerable<Type> types)
        {
            return new TypeScanningAssertions(Subject, types);
        }
    }
}
