using System;
using System.Collections.Generic;

namespace Autofac.TestingHelpers
{
    public static class Module<TModule> where TModule : Module, new()
    {
        public static IContainer GetTestContainer(
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
        {
            return new TModule().Container(arrange, types);
        }

        public static MockContainerBuilder GetTestBuilder(
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
        {
            return new TModule().Builder(arrange, types);
        }
    }
}
