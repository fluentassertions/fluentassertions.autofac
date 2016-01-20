using System;
using System.Collections.Generic;
using Autofac;

namespace FluentAssertions.Autofac
{
    public static class TestExtensions
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
            if (types != null)
                builder.Substitute(types);
            arrange?.Invoke(builder);
            builder.RegisterModule(module);
            return builder;
        }

        public static void Substitute(this ContainerBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
                Substitute(builder, type);
        }

        public static void Substitute(this ContainerBuilder builder, Type type)
        {
            builder.RegisterInstance(NSubstitute.Substitute.For(new[] { type }, new object[] { }))
                .AsImplementedInterfaces().AsSelf();
        }

        public static void Substitute<T>(this ContainerBuilder builder) where T : class
        {
            builder.RegisterInstance(NSubstitute.Substitute.For<T>())
                .AsImplementedInterfaces().AsSelf();
        }
    }
}