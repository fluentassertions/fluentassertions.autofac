using System;
using Autofac;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
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