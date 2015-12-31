using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;

namespace AutoFac.TestingHelpers
{
    [ExcludeFromCodeCoverage]
    [ExcludeFromConventions("testing helper")]
    public static class Module<TModule> where TModule : Module, new()
    {
        public static IContainer GetContainer(Action<ContainerBuilder> arrange = null)
        {
            return new TModule().Container(arrange);
        }

        public static MockContainerBuilder GetBuilder(Action<ContainerBuilder> arrange = null)
        {
            return new TModule().Builder(arrange);
        }
    }
}
