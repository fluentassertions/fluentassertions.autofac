using Autofac;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains extension methods for Autofac assertions.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public static class AutofacAssertionExtensions
    {
        /// <summary>
        ///     Returns an <see cref="ContainerAssertions" /> object that can be used to assert the current
        ///     <see cref="IContainer" />.
        /// </summary>
        public static ContainerAssertions Should(this IContainer container)
        {
            return new ContainerAssertions(container);
        }

        /// <summary>
        ///     Returns an <see cref="BuilderWrapperAssertions" /> object that can be used to assert the current
        ///     <see cref="BuilderWrapper" />.
        /// </summary>
        public static BuilderWrapperAssertions Should(this BuilderWrapper wrapper)
        {
            return new BuilderWrapperAssertions(wrapper);
        }
    }
}
