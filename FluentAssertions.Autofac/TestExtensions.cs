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
            where TModule : Module
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            var wrapper = WrapperFor(module, arrange);
            return wrapper.Builder.Build();
        }

        /// <summary>
        ///   Returns an <see cref="BuilderWrapper"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static BuilderWrapper WrapperFor<TModule>(this TModule module, Action<ContainerBuilder> arrange = null)
            where TModule : Module
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            var wrapper = new BuilderWrapper();
            arrange?.Invoke(wrapper.Builder);
            wrapper.Builder.RegisterModule(module);
            return wrapper;
        }

        /// <summary>
        ///   Returns an <see cref="IContainer"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static IContainer Container<TModule>(this TModule module, Action<ContainerBuilder, TModule> arrange)
            where TModule : Module
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            if (arrange == null) throw new ArgumentNullException(nameof(arrange));
            var wrapper = WrapperFor(module, arrange);
            return wrapper.Builder.Build();
        }

        /// <summary>
        ///   Returns an <see cref="MockContainerBuilder"/> suitable for testing the specified module.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static BuilderWrapper WrapperFor<TModule>(this TModule module, Action<ContainerBuilder, TModule> arrange)
            where TModule : Module
        {
            if (module == null) throw new ArgumentNullException(nameof(module));
            if (arrange == null) throw new ArgumentNullException(nameof(arrange));
            var wrapper = new BuilderWrapper();
            arrange.Invoke(wrapper.Builder, module);
            wrapper.Builder.RegisterModule(module);
            return wrapper;
        }
    }
}
