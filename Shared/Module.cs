﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains extension methods for Module assertions.
    /// </summary>
    /// <typeparam name="TModule"></typeparam>
    [DebuggerNonUserCode]
    public static class Module<TModule> where TModule : Module, new()
    {
        /// <summary>
        ///    Returns a test <see cref="IContainer"/> that can be used to assert the the specified <see typeparamref="TModule"/>.
        /// </summary>
        /// <param name="arrange">optional builder arrangement for the module</param>
        /// <param name="types">Types to substitute for the module</param>
        /// <returns></returns>
        public static IContainer GetTestContainer(
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
        {
            return new TModule().Container(arrange, types);
        }

        /// <summary>
        ///    Returns a test <see cref="MockContainerBuilder"/> that can be used to assert the the specified <see typeparamref="TModule"/>.
        /// </summary>
        /// <param name="arrange">optional builder arrangement for the module</param>
        /// <param name="types">Types to substitute for the module</param>
        /// <returns></returns>
        public static MockContainerBuilder GetTestBuilder(
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
        {
            return new TModule().Builder(arrange, types);
        }
    }
}