using System.Diagnostics;
using Autofac;

namespace FluentAssertions.Autofac
{
    [DebuggerNonUserCode]
    public static class AutofacAssertionExtensions
    {
        public static ContainerAssertions Should(this IContainer container)
        {
            return new ContainerAssertions(container);
        }

        public static MockContainerBuilderAssertions Should(this MockContainerBuilder builder)
        {
            return new MockContainerBuilderAssertions(builder);
        }
    }
}