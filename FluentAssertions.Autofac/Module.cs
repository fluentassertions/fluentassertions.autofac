using System;
using Autofac;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains extension methods for Module assertions.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public static class Module<TModule> where TModule : Module, new()
    {
        /// <summary>
        ///    Returns a test <see cref="IContainer"/> that can be used to assert the specified <see typeparamref="TModule"/>.
        /// </summary>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static IContainer GetTestContainer(Action<ContainerBuilder> arrange = null)
        {
            return new TModule().Container(arrange);
        }

        /// <summary>
        ///    Returns a test <see cref="IContainer"/> that can be used to assert the specified <see typeparamref="TModule"/>.
        /// </summary>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static IContainer GetTestContainer(Action<ContainerBuilder, TModule> arrange)
        {
            return new TModule().Container(arrange);
        }

        /// <summary>
        ///    Returns a test <see cref="MockContainerBuilder"/> that can be used to assert the specified <see typeparamref="TModule"/>.
        /// </summary>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static MockContainerBuilder GetTestBuilder(Action<ContainerBuilder> arrange = null)
        {
            return new TModule().Builder(arrange);
        }

        /// <summary>
        ///    Returns a test <see cref="MockContainerBuilder"/> that can be used to assert the specified <see typeparamref="TModule"/>.
        /// </summary>
        /// <param name="arrange">optional builder arrangement for the module</param>
        public static MockContainerBuilder GetTestBuilder(Action<ContainerBuilder, TModule> arrange)
        {
            return new TModule().Builder(arrange);
        }
    }
}
