using System;
using Autofac;
using FluentAssertions.Autofac;
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
            _container.ShouldHave().Registered(serviceType).As(contractType);
        }

        [Test]
        public void Register_SampleService()
        {
            _container.ShouldHave().Registered(typeof(SampleService)).As(typeof(ISampleService));
            _container.ShouldHave().Registered<SampleService>().As<ISampleService>();
        }

        [Test]
        public void Register_SampleInstance()
        {
            _container.ShouldHave().Registered(SampleModule.SampleInstance)
                .As<ISampleInstance>();
        }

        [Test]
        [TestCase(typeof(INamedInstance), "SampleName")]
        public void Register_Named(Type type, string name)
        {
            _container.ShouldHave().Registered<NamedInstance>().Named<INamedInstance>(name);
            _container.ShouldHave().Registered(typeof(NamedInstance)).Named(name, type);
        }

        [Test]
        [TestCase(typeof(IDeviceState), DeviceState.Online)]
        public void Register_Keyed(Type type, DeviceState key)
        {
            _container.ShouldHave().Registered<OnlineState>().Keyed<IDeviceState>(key);
            _container.ShouldHave().Registered(typeof(OnlineState)).Keyed(key, type);
        }

        [Test]
        public void AutoActivate_SampleStarter()
        {
            _container.ShouldHave().Registered<SampleStarter>().AutoActivate();
        }
    }
}