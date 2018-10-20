using System;
using Autofac;
using FluentAssertions.Autofac;
using Xunit;

namespace SampleLib
{
    // ReSharper disable once InconsistentNaming
    public class SampleModule_Should
    {
        private readonly IContainer _container = Module<SampleModule>.GetTestContainer();

        [Theory]
        [InlineData(typeof(SampleService), typeof(ISampleService))]
        public void Register(Type serviceType, Type contractType)
        {
            _container.Should().Have().Registered(serviceType).As(contractType);
        }

        [Fact]
        public void Register_SampleService()
        {
            _container.Should().Have().Registered(typeof(SampleService)).As(typeof(ISampleService));
            _container.Should().Have().Registered<SampleService>().As<ISampleService>();
        }

        [Fact]
        public void Register_SampleInstance()
        {
            _container.Should().Have().Registered(SampleModule.SampleInstance)
                .As<ISampleInstance>();
        }

        [Theory]
        [InlineData(typeof(INamedInstance), "SampleName")]
        public void Register_Named(Type type, string name)
        {
            _container.Should().Have().Registered<NamedInstance>().Named<INamedInstance>(name);
            _container.Should().Have().Registered(typeof(NamedInstance)).Named(name, type);
        }

        [Theory]
        [InlineData(typeof(IDeviceState), DeviceState.Online)]
        public void Register_Keyed(Type type, DeviceState key)
        {
            _container.Should().Have().Registered<OnlineState>().Keyed<IDeviceState>(key);
            _container.Should().Have().Registered(typeof(OnlineState)).Keyed(key, type);
        }

        [Fact]
        public void AutoActivate_SampleStarter()
        {
            _container.Should().Have().Registered<SampleStarter>().AutoActivate();
        }
    }
}
