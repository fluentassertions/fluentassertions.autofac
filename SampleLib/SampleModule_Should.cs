using System;
using Autofac;
using Autofac.TestingHelpers;
using NEdifis.Attributes;
using NUnit.Framework;

namespace SampleLib
{
    [TestFixtureFor(typeof(SampleModule))]
    // ReSharper disable once InconsistentNaming
    internal class SampleModule_Should
    {
        private readonly IContainer _container = Module<SampleModule>.GetTestContainer();

        [Test]
        [TestCase(typeof(SampleService), typeof(ISampleService))]
        public void Register(Type serviceType, Type contractType)
        {
            _container.Should().Have().Registered(serviceType).As(contractType);
        }

        [Test]
        public void Register_SampleService()
        {
            _container.Should().Have().Registered(typeof(SampleService)).As(typeof(ISampleService));
            _container.Should().Have().Registered<SampleService>().As<ISampleService>();
        }

        [Test]
        public void Register_SampleInstance()
        {
            _container.Should().Have().Registered(SampleModule.SampleInstance)
                .As<ISampleInstance>();
        }

        [Test]
        [TestCase(typeof(INamedInstance), "SampleName")]
        public void Register_Named(Type type, string name)
        {
            _container.Should().Have().Registered<NamedInstance>().Named<INamedInstance>(name);
            _container.Should().Have().Registered(typeof(NamedInstance)).Named(name, type);
        }

        [Test]
        [TestCase(typeof(IDeviceState), DeviceState.Online)]
        public void Register_Keyed(Type type, DeviceState key)
        {
            _container.Should().Have().Registered<OnlineState>().Keyed<IDeviceState>(key);
            _container.Should().Have().Registered(typeof(OnlineState)).Keyed(key, type);
        }


        [Test]
        public void AutoActivate_SampleStarter()
        {
            _container.Should().Have().Registered<SampleStarter>().AutoActivate();
        }
    }
}