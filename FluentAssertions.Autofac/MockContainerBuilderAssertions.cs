using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Primitives;
using Module = Autofac.Module;

namespace FluentAssertions.Autofac
{
    [DebuggerNonUserCode]
    public class MockContainerBuilderAssertions : ReferenceTypeAssertions<MockContainerBuilder, MockContainerBuilderAssertions>
    {
        protected override string Context => nameof(MockContainerBuilder);

        public MockContainerBuilderAssertions(MockContainerBuilder subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            Subject = subject;
        }

        public void RegisterModule<TModule>() where TModule : Module, new()
        {
            RegisterModule(typeof(TModule));
        }

        public void RegisterModule(Type moduleType)
        {
            var moduleCallback = Subject.Callbacks
                .FirstOrDefault(callback => callback.Target.GetType() == moduleType && callback.Method.Name == nameof(Module.Configure));
            moduleCallback.Should().NotBeNull($"Module '{moduleType}' should be registered");
        }

        public void RegisterModulesIn(Assembly assembly)
        {
            var moduleTypes = assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t)).ToList();
            moduleTypes.ForEach(RegisterModule);
        }
    }
}