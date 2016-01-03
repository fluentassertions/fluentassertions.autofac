using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;

namespace Autofac.TestingHelpers
{
    public class MockContainerBuilderAssertions
    {
        private readonly MockContainerBuilder _builder;

        public MockContainerBuilderAssertions(MockContainerBuilder builder)
        {
            _builder = builder;
        }

        public void RegisteredModule<TModule>() where TModule : Module, new()
        {
            RegisteredModule(typeof(TModule));
        }

        public void RegisteredModule(Type moduleType)
        {
            var moduleCallback = _builder.Callbacks
                .FirstOrDefault(callback => callback.Target.GetType() == moduleType && callback.Method.Name == nameof(Module.Configure));
            moduleCallback.Should().NotBeNull($"Module '{moduleType}' should be registered");
        }

        public void RegisteredModulesIn(Assembly assembly)
        {
            var moduleTypes = assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t)).ToList();
            moduleTypes.ForEach(RegisteredModule);
        }
    }
}