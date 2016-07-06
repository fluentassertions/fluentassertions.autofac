using System;
using Autofac;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof(TestExtensions))]
    // ReSharper disable InconsistentNaming
    internal class TestExtensions_Should
    {
        [Test]
        public void Provide_builder()
        {
            var module = new SampleModule();

            var builder = module.Builder();
            ((object) builder).Should().BeOfType<MockContainerBuilder>();

            builder.RegisterInstance(Substitute.For<ICloneable>());
            builder.RegisterInstance(Substitute.For<IComparable>());
            builder.RegisterInstance(Substitute.For<IConvertible>());
            builder.RegisterInstance(Substitute.For<ICustomFormatter>());

            var container = builder.Build();

            container.Resolve<IDisposable>().Should().NotBeNull();

            container.Resolve<ICloneable>().Should().NotBeNull();
            container.Resolve<IComparable>().Should().NotBeNull();
            container.Resolve<IConvertible>().Should().NotBeNull();
            container.Resolve<ICustomFormatter>().Should().NotBeNull();
        }

        [Test]
        public void Provide_container()
        {
            var module = new SampleModule();

            var container = module.Container(builder =>
            {
                builder.RegisterInstance(Substitute.For<ICloneable>());
                builder.RegisterInstance(Substitute.For<IConvertible>());
                builder.RegisterInstance(Substitute.For<ICustomFormatter>());
            });

            container.Resolve<IDisposable>().Should().NotBeNull();

            container.Resolve<ICloneable>().Should().NotBeNull();
            container.Resolve<IConvertible>().Should().NotBeNull();
            container.Resolve<ICustomFormatter>().Should().NotBeNull();
        }

        private class SampleModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterInstance(Substitute.For<IDisposable>());
            }
        }
    }
}