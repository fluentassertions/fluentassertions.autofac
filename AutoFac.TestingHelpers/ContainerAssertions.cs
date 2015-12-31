using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Register<TRegister>(string because = null)
        {
            Register(typeof(TRegister), because);
        }

        public void Register(Type type, string because = null)
        {
            _container.IsRegistered(type).Should().BeTrue(because ?? $"Type '{type}' should be registered but it is not.");
        }

        public void Register(IEnumerable<Type> types)
        {
            types.All(_container.IsRegistered).Should().BeTrue();
        }

        public ResolveAssertions<TRegister> Resolve<TRegister>()
        {
            Register<TRegister>();
            return new ResolveAssertions<TRegister>(_container);
        }

        public void AutoActivate<TResolve>() where TResolve : IStartable
        {
            Resolve<IStartable>().As<TResolve>();
        }

        public void AutoActivate(Type type, params Type[] moreTypes)
        {
            Resolve<IStartable>().As(type, moreTypes);
        }
    }
}