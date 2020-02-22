using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
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
        ///   The callbacks that have been registered on the builder.
        /// </summary>
        public IList<DeferredCallback> Callbacks { get; }

        /// <inheritdoc />
        public MockContainerBuilder()
        {
            var field = typeof(ContainerBuilder).GetField("_configurationCallbacks",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Callbacks = (IList<DeferredCallback>)field.GetValue(this);
        }

        /// <summary>
        /// Returns the modules registered to this <see cref="MockContainerBuilder"/>.
        /// </summary>
        public IEnumerable<Module> GetModules()
        {
            return Callbacks
                .Select(c => c.Callback)
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
