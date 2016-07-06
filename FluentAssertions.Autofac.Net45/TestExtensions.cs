using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains extension methods for Autofac Containers and Builder assertions.
    /// </summary>
    [DebuggerNonUserCode]
    public static class TestExtensions
    {
        /// <summary>
        ///   Returns an <see cref="IContainer"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        /// <param name="types">Types to substitute for the module</param>
        public static IContainer Container<TModule>(this TModule module,
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
            where TModule : Module, new()
        {
            var builder = Builder(module, arrange, types);
            return builder.Build();
        }

        /// <summary>
        ///   Returns an <see cref="MockContainerBuilder"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        /// <param name="types">Types to substitute for the module</param>
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

        /// <summary>
        ///   Register substitute instances for the specified types
        /// </summary>
        /// <param name="builder">The builder where to register the substitutes</param>
        /// <param name="types">The types to substitute</param>
        public static void Substitute(this ContainerBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
                Substitute(builder, type);
        }

        /// <summary>
        ///   Register a substitute instance for the specified type
        /// </summary>
        /// <param name="builder">The builder where to register the substitute</param>
        /// <param name="type">The type to substitute</param>
        public static void Substitute(this ContainerBuilder builder, Type type)
        {
            builder.RegisterInstance(NSubstitute.Substitute.For(new[] { type }, new object[] { }))
                .AsImplementedInterfaces().AsSelf();
        }

        /// <summary>
        ///   Register a substitute instance for the specified type
        /// </summary>
        /// <param name="builder">The builder where to register the substitute</param>
        /// <typeparam name="T">The type to substitute</typeparam>
        public static void Substitute<T>(this ContainerBuilder builder) where T : class
        {
            builder.RegisterInstance(NSubstitute.Substitute.For<T>())
                .AsImplementedInterfaces().AsSelf();
        }
    }
}