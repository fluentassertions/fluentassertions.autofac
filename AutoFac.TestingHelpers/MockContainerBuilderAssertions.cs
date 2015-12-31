using System.Linq;
using System.Reflection;
using FluentAssertions;
using Module = Autofac.Module;

namespace AutoFac.TestingHelpers
{
    public class MockContainerBuilderAssertions
    {
        private readonly MockContainerBuilder _builder;

        public MockContainerBuilderAssertions(MockContainerBuilder builder)
        {
            _builder = builder;
        }

        public void RegisterAllModulesInAssembly(Assembly assembly)
        {
            var expectedModules = assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t)).ToArray();

            var callbacks = _builder.Callbacks;
            callbacks.Count.Should().BeGreaterOrEqualTo(expectedModules.Length);

            foreach (var callback in callbacks)
            {
                var module = callback.Target as Module;
                if (module != null) callback.Method.Name.Should().Be(nameof(Module.Configure));
            }
        }
    }
}