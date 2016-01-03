using NEdifis.Attributes;
using NUnit.Framework;

namespace Autofac.TestingHelpers
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

            sut.Should().RegisteredModulesIn(assembly);
            sut.Should().RegisteredModule<SampleModule>();
        }

        [Test]
        public void Support_testing_module_registration()
        {
            var sut = new MockContainerBuilder();
            sut.RegisterModule<SampleModule>();
            sut.Should().RegisteredModule<SampleModule>();
            sut.Should().RegisteredModule(typeof(SampleModule));
        }

        public class SampleModule : Module
        { }
    }
}