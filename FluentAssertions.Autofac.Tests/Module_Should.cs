using System.Diagnostics;
using Autofac;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class Module_Should
    {
        [Fact]
        public void Provide_test_container()
        {
            var container = Module<SampleModule>.GetTestContainer((builder,module) =>
            {
                Trace.WriteLine($"Customizing '{builder}' and '{module}'.");
            });
            container.Should().NotBeNull();
        }

        [Fact]
        public void Provide_test_builder()
        {
            var builder = Module<SampleModule>.GetTestBuilder((b,m) =>
            {
                Trace.WriteLine($"Customizing '{b}' and '{m}'.");
            });
            builder.Should().RegisterModule<SampleModule>();
        }

        private class SampleModule : Module { }
    }
}
