using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;

namespace AutoFac.TestingHelpers
{
    public class RegisterAssertions<TRegister> : RegisterAssertions
    {
        public RegisterAssertions(IContainer container)
            : base(container, typeof(TRegister))
        {
        }

        public RegisterAssertions<TRegister> As<TResolve>()
        {
            var instances = Container.Resolve<IEnumerable<TResolve>>().ToArray();
            var instance = instances.OfType<TRegister>().FirstOrDefault();
            instance.Should().NotBeNull($"Type '{typeof(TRegister)}' should be registered as '{typeof(TResolve)}'");
            return this;
        }
    }

    public class RegisterAssertions
    {
        private readonly Type _type;
        protected readonly IContainer Container;

        public RegisterAssertions(IContainer container, Type type) 
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            Container = container;
            _type = type;
        }

        public RegisterAssertions As(Type type, string because = null)
        {
            var actual = Container.Resolve(type);
            actual.Should().BeOfType(_type, because ?? $"Type '{_type}' should be registered as '{type}'");
            return this;
        }

        public RegisterAssertions Named<TService>(string name, string because = null)
        {
            return Named(name, typeof(TService), because);
        }

        public RegisterAssertions Named(string name, Type type, string because = null)
        {
            Container.IsRegisteredWithName(name, type)
                .Should().BeTrue(because ?? $"Type '{type}' should be registered with name '{name}'");
            return this;
        }

        public RegisterAssertions Keyed<TService>(object key, string because = null)
        {
            return Keyed(key, typeof(TService), because);
        }

        public RegisterAssertions Keyed(object key, Type type, string because = null)
        {
            Container.IsRegisteredWithKey(key, type)
                .Should().BeTrue(because ?? $"Type '{type}' should be registered with key '{key}'");
            return this;
        }
    }
}