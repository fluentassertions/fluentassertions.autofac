using System;
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
        public static IContainer Container<TModule>(this TModule module, Action<ContainerBuilder> arrange = null)
            where TModule : Module, new()
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            var builder = Builder(module, arrange);
            return builder.Build();
        }

        /// <summary>
        ///   Returns an <see cref="MockContainerBuilder"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static MockContainerBuilder Builder<TModule>(this TModule module, Action<ContainerBuilder> arrange = null)
            where TModule : Module, new()
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            var builder = new MockContainerBuilder();
            arrange?.Invoke(builder);
            builder.RegisterModule(module);
            return builder;
        }

        /// <summary>
        ///   Returns an <see cref="IContainer"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static IContainer Container<TModule>(this TModule module, Action<ContainerBuilder, TModule> arrange)
            where TModule : Module, new()
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            if (arrange == null) throw new ArgumentNullException(nameof(arrange));
            var builder = Builder(module, arrange);
            return builder.Build();
        }

        /// <summary>
        ///   Returns an <see cref="MockContainerBuilder"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static MockContainerBuilder Builder<TModule>(this TModule module, Action<ContainerBuilder, TModule> arrange)
            where TModule : Module, new()
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            if (arrange == null) throw new ArgumentNullException(nameof(arrange));
            var builder = new MockContainerBuilder();
            arrange.Invoke(builder, module);
            builder.RegisterModule(module);
            return builder;
        }
    }
}