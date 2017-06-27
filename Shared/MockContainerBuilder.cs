using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Module = Autofac.Module;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///   A testable <see cref="ContainerBuilder"/> that exposes the callbacks registered on the builder.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class MockContainerBuilder : ContainerBuilder
    {
        /// <summary>
        /// Register a callback that will be invoked when the container is configured.
        /// </summary>
        /// <remarks>
        /// This is primarily for extending the builder syntax.
        /// </remarks>
        /// <param name="configurationCallback">Callback to execute.</param>
        public override DeferredCallback RegisterCallback(Action<IComponentRegistry> configurationCallback)
        {
            Callbacks.Add(configurationCallback);
            return base.RegisterCallback(configurationCallback);
        }

        /// <summary>
        ///   The callbacks that have been registered on the builder.
        /// </summary>
        public List<Action<IComponentRegistry>> Callbacks { get; } = new List<Action<IComponentRegistry>>();

        /// <summary>
        /// Returns the modules registered to this <see cref="MockContainerBuilder"/>.
        /// </summary>
        public IEnumerable<Module> GetModules()
        {
            return Callbacks
                .Where(callback => callback.Target is Module
                    && callback.GetMethodInfo().Name == nameof(Module.Configure))
                .Select(callback => (Module)callback.Target);
        }

        private static readonly MethodInfo LoadModule = typeof(Module).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Executes the Load-method of the specified <see cref="Module"/> on this <see cref="MockContainerBuilder"/>.
        /// </summary>
        /// <param name="module">The module to load</param>
        public void Load(Module module)
        {
            LoadModule.Invoke(module, new object[] { this });
        }
    }
}