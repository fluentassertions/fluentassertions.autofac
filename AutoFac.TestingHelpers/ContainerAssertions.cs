using System;
using Autofac;
using FluentAssertions;

namespace AutoFac.TestingHelpers
{
    public class ContainerAssertions
    {
        private readonly IContainer _container;

        public ContainerAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public RegisterAssertions<TService> RegisterType<TService>()
        {
            return new RegisterAssertions<TService>(_container);
        }

        public RegisterAssertions RegisterType(Type type)
        {
            return new RegisterAssertions(_container, type);
        }

        /*
        public RegisterAssertions RegisterInstance(object instance)
        {
            return new RegisterAssertions(_container, instance);
        }*/

        public void NotRegister<TService>(string because = null)
        {
            NotRegister(typeof(TService), because);
        }

        public void NotRegister(Type type, string because = null)
        {
            _container.IsRegistered(type).Should()
                .BeFalse(because ?? $"Type '{type}' should not be registered but it is.");
        }

        public void NotRegisterNamed<TService>(string name, string because = null)
        {
            NotRegisterNamed(name, typeof(TService), because);
        }

        public void NotRegisterNamed(string name, Type type, string because = null)
        {
            _container.IsRegisteredWithName(name, type).Should()
                .BeFalse(because ?? $"Type '{type}' should not be registered with name '{name}' but it is.");
        }

        /*
        public RegisterAssertions<IStartable> AutoActivate<TResolve>() where TResolve : IStartable
        {
            return new RegisterAssertions<IStartable>(_container).As<TResolve>();
        }

        public RegisterAssertions<IStartable> AutoActivate(Type type, params Type[] moreTypes)
        {
            return new RegisterAssertions<IStartable>(_container).As(type, moreTypes);
        }*/
    }
}