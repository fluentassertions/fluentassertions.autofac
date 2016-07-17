using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof(MockContainerBuilderAssertions))]
    // ReSharper disable InconsistentNaming
    internal class MockContainerBuilderAssertions_Should
    {
        [Test]
        public void Support_testing_assembly_modules_registrations()
        {
            var sut = new MockContainerBuilder();

            var assembly = typeof(MockContainerBuilderAssertions_Should).Assembly;
            sut.RegisterAssemblyModules(assembly);

            sut.Should().RegisterModulesIn(assembly);
            sut.Should().RegisterModule<SampleModule>();
            sut.Should().RegisterModule<SampleModule2>();
        }

        [Test]
        public void Support_testing_module_registration()
        {
            var builder = new MockContainerBuilder();
            builder.RegisterModule<SampleModule>();

            var builderShould = builder.Should();
            builderShould.RegisterModule<SampleModule>();
            builderShould.RegisterModule(typeof(SampleModule));

            builderShould.RegisterModule<SampleModule2>();
        }

        public class SampleModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterModule<SampleModule2>();
            }
        }

        public class SampleModule2 : Module { }
    }
}