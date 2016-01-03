using System;
using NEdifis.Attributes;

namespace Autofac.TestingHelpers
{
    [ExcludeFromConventions("implicitly tested")]
    public class ContainerAssertions
    {
        private readonly IContainer _container;

        public ContainerAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public ResolveAssertions<TService> Resolve<TService>()
        {
            return new ResolveAssertions<TService>(_container);
        }

        public ContainerRegistrationAssertions Have()
        {
            return new ContainerRegistrationAssertions(_container);
        }
    }
}