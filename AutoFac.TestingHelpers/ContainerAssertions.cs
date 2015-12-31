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
            _container = container;
        }

        public void Resolve<TRegister>(params Type[] resolvedTypes)
        {
            _container.IsRegistered<TRegister>().Should().BeTrue();

            if (resolvedTypes == null || !resolvedTypes.Any()) return;

            var instances = _container.Resolve<IEnumerable<TRegister>>().ToArray();
            foreach (var type in resolvedTypes)
            {
                var instance = instances.FirstOrDefault(i => i.GetType() == type);
                instance.Should().NotBeNull($"Type '{type}' should be registered as '{typeof(TRegister)}'");
            }
        }

        public void Resolve<TRegister, TResolve>() where TResolve : TRegister
        {
            Resolve<TRegister>(typeof(TResolve));
        }

        public void ResolveTo<TRegister>(object expected)
        {
            _container.Resolve<TRegister>().Should().Be(expected);
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

        public void AutoActivate<TRegister>() where TRegister : IStartable
        {
            Resolve<IStartable, TRegister>();
        }

        public void AutoActivate(params Type[] types)
        {
            Resolve<IStartable>(types);
        }
    }
}