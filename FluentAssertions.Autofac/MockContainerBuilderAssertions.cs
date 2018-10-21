using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Module = Autofac.Module;

namespace FluentAssertions.Autofac
{
    /// <inheritdoc />
    /// <summary>
    ///     Contains a number of methods to assert that an <see cref="T:FluentAssertions.Autofac.MockContainerBuilder" /> is in the expected state.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class MockContainerBuilderAssertions : ReferenceTypeAssertions<MockContainerBuilder, MockContainerBuilderAssertions>
    {
        /// <inheritdoc />
        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
#if !NETSTANDARD_1X
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
        protected override string Identifier => nameof(MockContainerBuilder);

        /// <summary>
        ///    Initializes a new instance of the <see cref="MockContainerBuilderAssertions" /> class.
        /// </summary>
        /// <param name="subject"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MockContainerBuilderAssertions(MockContainerBuilder subject)
        {
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            _modules = Subject.GetModules().ToList();
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
            Traverse();
            var module = _modules.FirstOrDefault(m => m.GetType() == moduleType);
            Execute.Assertion
                .ForCondition(module != null)
                .FailWith($"Module '{moduleType}' should be registered but it was not.");
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

        private bool _traversed;
        private readonly List<Module> _modules;

        private void Traverse()
        {
            if (_traversed) return;

            var builder = new MockContainerBuilder();
            _modules.ForEach(module => builder.Load(module));
            var traversedModules = builder.GetModules();
            _modules.AddRange(traversedModules);

            _traversed = true;
        }
    }
}
