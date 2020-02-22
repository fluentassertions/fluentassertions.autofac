using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac.Core.Registration;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable once InconsistentNaming
    public class MockContainerBuilder_Should
    {
        [Fact]
        public void Be_Empty_At_Start()
        {
            var sut = new MockContainerBuilder();
            sut.Callbacks.Should().BeEmpty();
        }

        [Fact]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void Register_And_Return_Callbacks()
        {
            var sut = new MockContainerBuilder();

            Action<IComponentRegistryBuilder> cb1 = registry => { };
            sut.RegisterCallback(cb1);
            sut.Callbacks.Single().Callback.Should().Be(cb1);

            Action<IComponentRegistryBuilder> cb2 = registry => { };
            sut.RegisterCallback(cb2);
            sut.Callbacks.Last().Callback.Should().Be(cb2);
        }
    }
}
