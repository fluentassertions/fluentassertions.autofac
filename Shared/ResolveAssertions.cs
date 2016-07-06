using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    [DebuggerNonUserCode]
    public class ResolveAssertions<TService> : ReferenceTypeAssertions<IContainer, ResolveAssertions<TService>>
    {
        private readonly List<TService> _instances;

        public ResolveAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            Subject = container;
            _instances = Subject.Resolve<IEnumerable<TService>>().ToList();

            Execute.Assertion
                .ForCondition(_instances.Any())
                .FailWith($"Expected container to resolve '{typeof(TService)}' but it did not.");
        }

        public RegistrationAssertions As<TImplementation>()
            where TImplementation : TService
        {
            return As(typeof (TImplementation));
        }

        public RegistrationAssertions AsSelf()
        {
            return As<TService>();
        }

        public RegistrationAssertions As(Type type)
        {
            _instances.Should().Contain(instance => instance.GetType() == type,
                $"Type '{typeof (TService)}' should be resolved as '{type}'");

            return new RegistrationAssertions(Subject, type);
        }

        protected override string Context => nameof(IContainer);
    }
}