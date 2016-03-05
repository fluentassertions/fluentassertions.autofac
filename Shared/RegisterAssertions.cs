using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;

namespace FluentAssertions.Autofac
{
    [DebuggerNonUserCode]
    public class RegisterAssertions : RegistrationAssertions
    {
        public RegisterAssertions(IContainer subject, Type type) : base(subject, type)
        {
        }

        public RegisterAssertions As<TResolve>()
        {
            var instances = Subject.Resolve<IEnumerable<TResolve>>().ToArray();
            var resolved = instances.FirstOrDefault(instance => instance.GetType() == Type);
            resolved.Should().NotBeNull($"Type '{Type}' should be registered as '{typeof (TResolve)}'");
            return this;
        }

        public RegisterAssertions As(Type type)
        {
            var actual = Subject.Resolve(type);
            actual.Should().BeOfType(Type, $"Type '{Type}' should be registered as '{type}'");
            return this;
        }

        public RegisterAssertions AsSelf()
        {
            return As(Type);
        }
    }
}