using System;
using Autofac.Core;
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
        public void Register_And_Return_Callbacks()
        {
            var sut = new MockContainerBuilder();

            Action<IComponentRegistry> cb1 = registry => { };
            sut.RegisterCallback(cb1);
            sut.Callbacks.Should().HaveCount(1);
            sut.Callbacks.Should().OnlyContain(action => action == cb1);

            Action<IComponentRegistry> cb2 = registry => { };
            sut.RegisterCallback(cb2);
            sut.Callbacks.Should().HaveCount(2);
            sut.Callbacks.Should().OnlyContain(action => action == cb1||action==cb2);
        }
    }
}
