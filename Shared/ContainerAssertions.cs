using System;
using System.Diagnostics;
using Autofac;
using FluentAssertions.Primitives;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [DebuggerNonUserCode]
    public class ContainerAssertions : ReferenceTypeAssertions<IContainer, ContainerAssertions>
    {
        protected override string Context => nameof(IContainer);

        public ContainerAssertions(IContainer container)
        {
            Subject = container;
        }

        public ContainerRegistrationAssertions Have()
        {
            return new ContainerRegistrationAssertions(Subject);
        }

        public ResolveAssertions<TService> Resolve<TService>()
        {
            return new ResolveAssertions<TService>(Subject);
        }
    }

    [TestFixtureFor(typeof (ContainerAssertions))]
    // ReSharper disable InconsistentNaming
    internal class ContainerAssertions_Should
    {
        [Test]
        public void Provide_assertions()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(Substitute.For<IDisposable>());
            var container = builder.Build();

            var sut = container.Should();

            sut.Should().BeOfType<ContainerAssertions>();
            sut.Have().Should().BeOfType<ContainerRegistrationAssertions>();
            sut.Resolve<IDisposable>().Should().BeOfType<ResolveAssertions<IDisposable>>();
        }
    }
}
 