using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace AutoFac.TestingHelpers
{
    [TestFixtureFor(typeof(AssertionExtensions))]
    // ReSharper disable InconsistentNaming
    internal class AssertionExtensions_Should
    {
        [Test]
        public void Provide_extensions()
        {
            var builder = new MockContainerBuilder();
            builder.Should().Should().BeOfType<MockContainerBuilderAssertions>();

            var container = builder.Build();
            container.Should().Should().BeOfType<ContainerAssertions>();
        }
    }
}