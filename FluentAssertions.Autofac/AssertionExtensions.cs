using Autofac;

namespace FluentAssertions.Autofac
{
    public static class AssertionExtensions
    {
        public static ResolveAssertions<TService> ShouldResolve<TService>(this IContainer container)
        {
            return new ResolveAssertions<TService>(container);
        }

        public static ContainerRegistrationAssertions ShouldHave(this IContainer container)
        {
            return new ContainerRegistrationAssertions(container);
        }

        public static MockContainerBuilderAssertions ShouldHave(this MockContainerBuilder builder)
        {
            return new MockContainerBuilderAssertions(builder);
        }
    }
}