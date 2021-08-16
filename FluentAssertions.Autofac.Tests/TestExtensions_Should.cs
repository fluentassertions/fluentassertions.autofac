using System;
using Autofac;
using NSubstitute;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class TestExtensions_Should
    {
        [Fact]
        public void Provide_builder()
        {
            var module = new SampleModule();

            var wrapper = module.WrapperFor();
            ((object)wrapper).Should().BeOfType<BuilderWrapper>();

            var builder = wrapper.Builder;
            builder.RegisterInstance(Substitute.For<IComparable>());
            builder.RegisterInstance(Substitute.For<IConvertible>());
            builder.RegisterInstance(Substitute.For<ICustomFormatter>());

            var container = builder.Build();

            container.Resolve<IDisposable>().Should().NotBeNull();
            container.Resolve<IComparable>().Should().NotBeNull();
            container.Resolve<IConvertible>().Should().NotBeNull();
            container.Resolve<ICustomFormatter>().Should().NotBeNull();
        }

        [Fact]
        public void Provide_generic_module_builder()
        {
            var wrapper = Module<SampleModule>.GetTestBuilderWrapper(b =>
            {
                // register module dependency substitutes here!
                b.RegisterInstance(Substitute.For<IComparable>());
            });
            wrapper.Should().RegisterModule<SampleModule>();

            var container = wrapper.Builder.Build();

            container.Resolve<IDisposable>().Should().NotBeNull();
            container.Resolve<IComparable>().Should().NotBeNull();
        }

        [Fact]
        public void Provide_container()
        {
            var module = new SampleModule();

            var container = module.Container(builder =>
            {
                builder.RegisterInstance(Substitute.For<IConvertible>());
                builder.RegisterInstance(Substitute.For<ICustomFormatter>());
            });

            container.Resolve<IDisposable>().Should().NotBeNull();
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
