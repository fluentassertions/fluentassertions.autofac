using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;
using Module = Autofac.Module;

namespace AutoFac.TestingHelpers
{
    [ExcludeFromCodeCoverage]
    [ExcludeFromConventions("testing helper")]
    public static class AssertionExtensions
    {
        public static IContainer Container<TModule>(this TModule module, 
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
            where TModule : Module, new()
        {
            var builder = Builder(module, arrange, types);
            return builder.Build();
        }

        public static MockContainerBuilder Builder<TModule>(this TModule module, 
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
            where TModule : Module, new()
        {
            var builder = new MockContainerBuilder();
            if (types != null) builder.RegisterSubstitutes(types);
            arrange?.Invoke(builder);
            builder.RegisterModule(module);
            return builder;
        }

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