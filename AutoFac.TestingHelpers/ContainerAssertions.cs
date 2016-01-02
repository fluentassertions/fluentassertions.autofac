using System;
using FluentAssertions;

namespace Autofac.TestingHelpers
{
    public class ContainerAssertions
    {
        private readonly IContainer _container;

        public ContainerAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public RegisterAssertions RegisterType<TService>()
        {
            return new RegisterAssertions(_container, typeof(TService));
        }

        public RegisterAssertions RegisterType(Type type)
        {
            return new RegisterAssertions(_container, type);
        }

        public RegisterAssertions RegisterInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return new RegisterAssertions(_container, instance.GetType());
        }

        public void NotRegister<TService>(string because = null)
        {
            NotRegister(typeof(TService), because);
        }

        public void NotRegister(Type type, string because = null)
        {
            _container.IsRegistered(type).Should()
                .BeFalse(because ?? $"Type '{type}' should not be registered");
        }

        public void NotRegisterNamed<TService>(string name, string because = null)
        {
            NotRegisterNamed(name, typeof(TService), because);
        }

        public void NotRegisterNamed(string name, Type type, string because = null)
        {
            _container.IsRegisteredWithName(name, type).Should()
                .BeFalse(because ?? $"Type '{type}' should not be registered with name '{name}'");
        }

        public void NotRegisterKeyed<TService>(object key, string because = null)
        {
            NotRegisterKeyed(key, typeof(TService), because);
        }

        public void NotRegisterKeyed(object key, Type type, string because = null)
        {
            _container.IsRegisteredWithKey(key, type).Should()
                .BeFalse(because ?? $"Type '{type}' should not be registered with key '{key}'");
        }
    }
}