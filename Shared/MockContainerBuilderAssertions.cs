using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Primitives;
using Module = Autofac.Module;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="MockContainerBuilder" /> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class MockContainerBuilderAssertions : ReferenceTypeAssertions<MockContainerBuilder, MockContainerBuilderAssertions>
    {
        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
#if !PORTABLE && !CORE_CLR
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        protected override string Context => nameof(MockContainerBuilder);

        /// <summary>
        ///    Initializes a new instance of the <see cref="MockContainerBuilderAssertions" /> class.
        /// </summary>
        /// <param name="subject"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MockContainerBuilderAssertions(MockContainerBuilder subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            Subject = subject;
        }

        /// <summary>
        ///    Asserts that the specified module type has been registered on the current <see cref="MockContainerBuilder"/>.
        /// </summary>
        /// <typeparam name="TModule">The module type</typeparam>
        public void RegisterModule<TModule>() where TModule : Module, new()
        {
            RegisterModule(typeof(TModule));
        }

        /// <summary>
        ///    Asserts that the specified module type has been registered on the current <see cref="MockContainerBuilder"/>.
        /// </summary>
        /// <param name="moduleType">The module type</param>
        public void RegisterModule(Type moduleType)
        {
            var moduleCallback = Subject.Callbacks
                .FirstOrDefault(callback => callback.Target.GetType() == moduleType && callback.Method.Name == nameof(Module.Configure));
            moduleCallback.Should().NotBeNull($"Module '{moduleType}' should be registered");
        }

        /// <summary>
        ///    Asserts that the modules contained in the specified assembly have been registered on the current <see cref="MockContainerBuilder"/>.
        /// </summary>
        /// <param name="assembly">The module assembly</param>
        public void RegisterModulesIn(Assembly assembly)
        {
            var moduleTypes = assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t)).ToList();
            moduleTypes.ForEach(RegisterModule);
        }
    }
}