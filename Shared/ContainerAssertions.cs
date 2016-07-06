using System.Diagnostics;
using Autofac;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="IContainer" /> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ContainerAssertions : ReferenceTypeAssertions<IContainer, ContainerAssertions>
    {
        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
#if !PORTABLE && !CORE_CLR
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        protected override string Context => nameof(IContainer);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerAssertions" /> class.
        /// </summary>
        /// <param name="container">The subject</param>
        public ContainerAssertions(IContainer container)
        {
            Subject = container;
        }

        /// <summary>
        ///     Returns an <see cref="ContainerRegistrationAssertions"/> object that can be used to assert the current <see cref="IContainer"/>.
        /// </summary>
        public ContainerRegistrationAssertions Have()
        {
            return new ContainerRegistrationAssertions(Subject);
        }

        /// <summary>
        ///     Returns an <see cref="ResolveAssertions{TService}"/> object that can be used to assert the current <see typeparamref="TService"/>.
        /// </summary>
        public ResolveAssertions<TService> Resolve<TService>()
        {
            return new ResolveAssertions<TService>(Subject);
        }
    }
}
 