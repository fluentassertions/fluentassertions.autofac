using System;
using Autofac;

namespace FluentAssertions.Autofac
{
    public class ContainerRegistrationAssertions
    {
        private readonly IContainer _container;

        public ContainerRegistrationAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public RegisterAssertions Registered<TService>()
        {
            return new RegisterAssertions(_container, typeof(TService));
        }

        public RegisterAssertions Registered(Type type)
        {
            return new RegisterAssertions(_container, type);
        }

        public RegisterAssertions Registered(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return new RegisterAssertions(_container, instance.GetType());
        }

        public void NotRegistered<TService>()
        {
            NotRegistered(typeof(TService));
        }

        public void NotRegistered(Type type)
        {
            _container.IsRegistered(type).Should().BeFalse($"Type '{type}' should not be registered");
        }

        public void NotRegistered<TService>(string serviceName)
        {
            NotRegistered(serviceName, typeof(TService));
        }

        public void NotRegistered(string serviceName, Type type)
        {
            _container.IsRegisteredWithName(serviceName, type).Should()
                .BeFalse($"Type '{type}' should not be registered with name '{serviceName}'");
        }

        public void NotRegistered<TService>(object serviceKey)
        {
            NotRegistered(serviceKey, typeof(TService));
        }

        public void NotRegistered(object serviceKey, Type type)
        {
            _container.IsRegisteredWithKey(serviceKey, type).Should()
                .BeFalse($"Type '{type}' should not be registered with key '{serviceKey}'");
        }
    }
}