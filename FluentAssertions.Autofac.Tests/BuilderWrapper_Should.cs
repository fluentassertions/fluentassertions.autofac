using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac.Core.Registration;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable once InconsistentNaming
    public class BuilderWrapper_Should
    {
        [Fact]
        public void Be_Empty_At_Start()
        {
            var wrapper = new BuilderWrapper();
            wrapper.Callbacks.Should().BeEmpty();
        }

        [Fact]
        [SuppressMessage("ReSharper", "ConvertToLocalFunction")]
        public void Register_And_Return_Callbacks()
        {
            var wrapper = new BuilderWrapper();

            Action<IComponentRegistryBuilder> cb1 = registry => { };
            wrapper.Builder.RegisterCallback(cb1);
            wrapper.Callbacks.Single().Callback.Should().Be(cb1);

            Action<IComponentRegistryBuilder> cb2 = registry => { };
            wrapper.Builder.RegisterCallback(cb2);
            wrapper.Callbacks.Last().Callback.Should().Be(cb2);
        }
    }
}
