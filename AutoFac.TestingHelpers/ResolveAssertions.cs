using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using FluentAssertions;

namespace AutoFac.TestingHelpers
{
    public class ResolveAssertions<TRegister>
    {
        private readonly IContainer _container;

        public ResolveAssertions(IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            _container = container;
        }

        public void To(object expected, string because = "")
        {
            _container.Resolve<TRegister>().Should().Be(expected, because);
        }

        public void As<TResolve>() where TResolve : TRegister
        {
            As(typeof(TResolve));
        }

        public void As(Type type, params Type[] moreTypes)
        {
            var resolvedTypes = new List<Type> {type};
            if (moreTypes != null && moreTypes.Length > 0)
                resolvedTypes.AddRange(moreTypes);

            var instances = _container.Resolve<IEnumerable<TRegister>>().ToArray();
            foreach (var resolvedType in resolvedTypes)
            {
                var instance = instances.FirstOrDefault(i => i.GetType() == resolvedType);
                instance.Should().NotBeNull($"Type '{resolvedType}' should be registered as '{typeof(TRegister)}'");
            }
        }
    }
}