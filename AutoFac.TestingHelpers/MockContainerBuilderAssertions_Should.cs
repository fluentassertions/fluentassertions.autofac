using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace AutoFac.TestingHelpers
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

            sut.Should().RegisterAllModulesInAssembly(assembly);
            sut.Should().RegisterModule<SampleModule>();
        }

        [Test]
        public void Support_testing_module_registration()
        {
            var sut = new MockContainerBuilder();
            sut.RegisterModule<SampleModule>();
            sut.Should().RegisterModule<SampleModule>();
            sut.Should().RegisterModule(typeof(SampleModule));
        }

        public class SampleModule : Module
        { }
    }
}