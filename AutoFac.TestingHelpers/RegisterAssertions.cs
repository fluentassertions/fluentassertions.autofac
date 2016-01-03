using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace Autofac.TestingHelpers
{
    public class RegisterAssertions : RegistrationAssertions
    {
        public RegisterAssertions(IContainer container, Type type) : base(container, type)
        {
        }

        public RegisterAssertions As<TResolve>()
        {
            var instances = Container.Resolve<IEnumerable<TResolve>>().ToArray();
            var resolved = instances.FirstOrDefault(instance => instance.GetType() == Type);
            resolved.Should().NotBeNull($"Type '{Type}' should be registered as '{typeof (TResolve)}'");
            return this;
        }

        public RegisterAssertions As(Type type)
        {
            var actual = Container.Resolve(type);
            actual.Should().BeOfType(Type, $"Type '{Type}' should be registered as '{type}'");
            return this;
        }

        public RegisterAssertions AsSelf()
        {
            return As(Type);
        }
    }
}