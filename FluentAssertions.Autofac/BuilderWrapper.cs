using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Module = Autofac.Module;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     A testable <see cref="ContainerBuilder" /> that exposes the callbacks registered on the builder.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class BuilderWrapper
    {
        /// <summary>
        ///     The callbacks that have been registered on the builder.
        /// </summary>
        public IList<DeferredCallback> Callbacks { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerBuilder" /> class.
        /// </summary>
        public BuilderWrapper()
        {
            Builder = new ContainerBuilder();
            var field = typeof(ContainerBuilder).GetField("_configurationCallbacks",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
            {
                throw new InvalidOperationException("Could not access builder callbacks");
            }

            Callbacks = (IList<DeferredCallback>)field.GetValue(Builder);
        }

        /// <summary>
        ///     Returns the modules registered to the wrapped <see cref="ContainerBuilder" />.
        /// </summary>
        public IEnumerable<Module> GetModules()
        {
            return Callbacks
                .Select(c => c.Callback)
                .Where(callback => callback.Target is Module
                                   && callback.GetMethodInfo().Name == nameof(Module.Configure))
                .Select(callback => (Module)callback.Target);
        }

        private static readonly MethodInfo LoadModule =
            typeof(Module).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        ///     The builder
        /// </summary>
        public ContainerBuilder Builder { get; }

        /// <summary>
        ///     Executes the Load-method of the specified <see cref="Module" /> on the wrapped <see cref="ContainerBuilder" />.
        /// </summary>
        /// <param name="module">The module to load</param>
        public void Load(Module module)
        {
            LoadModule.Invoke(module, new object[] { Builder });
        }
    }
}
