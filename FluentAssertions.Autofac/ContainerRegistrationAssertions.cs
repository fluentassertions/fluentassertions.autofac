using System;
using System.Diagnostics;
using Autofac;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    [DebuggerNonUserCode]
    public class ContainerRegistrationAssertions : ReferenceTypeAssertions<IContainer, ContainerRegistrationAssertions>
    {
        protected override string Context => nameof(IContainer);

        public ContainerRegistrationAssertions(IContainer subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            Subject = subject;
        }

        public RegisterAssertions Registered<TService>()
        {
            return new RegisterAssertions(Subject, typeof(TService));
        }

        public RegisterAssertions Registered(Type type)
        {
            return new RegisterAssertions(Subject, type);
        }

        public RegisterAssertions Registered(object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            return new RegisterAssertions(Subject, instance.GetType());
        }

        public void NotRegistered<TService>()
        {
            NotRegistered(typeof(TService));
        }

        public void NotRegistered(Type type)
        {
            Subject.IsRegistered(type).Should().BeFalse($"Type '{type}' should not be registered");
        }

        public void NotRegistered<TService>(string serviceName)
        {
            NotRegistered(serviceName, typeof(TService));
        }

        public void NotRegistered(string serviceName, Type type)
        {
            Subject.IsRegisteredWithName(serviceName, type).Should()
                .BeFalse($"Type '{type}' should not be registered with name '{serviceName}'");
        }

        public void NotRegistered<TService>(object serviceKey)
        {
            NotRegistered(serviceKey, typeof(TService));
        }

        public void NotRegistered(object serviceKey, Type type)
        {
            Subject.IsRegisteredWithKey(serviceKey, type).Should()
                .BeFalse($"Type '{type}' should not be registered with key '{serviceKey}'");
        }
    }
}