using Autofac;

namespace AutoFac.TestingHelpers
{
    public static class AssertionExtensions
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