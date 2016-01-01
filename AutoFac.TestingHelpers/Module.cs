using System;
using NEdifis.Attributes;

namespace Autofac.TestingHelpers
{
    [ExcludeFromConventions("testing helper")]
    public static class Module<TModule> where TModule : Module, new()
    {
        public static IContainer GetTestContainer(Action<ContainerBuilder> arrange = null)
        {
            return new TModule().Container(arrange);
        }

        public static MockContainerBuilder GetTestBuilder(Action<ContainerBuilder> arrange = null)
        {
            return new TModule().Builder(arrange);
        }
    }
}
