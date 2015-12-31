using System;
using Autofac;
using AutoFac.TestingHelpers;
using NEdifis.Attributes;
using NUnit.Framework;

namespace SampleLib
{
    [TestFixtureFor(typeof(SampleModule))]
    // ReSharper disable once InconsistentNaming
    internal class SampleModule_Should
    {
        private readonly IContainer _container = Module<SampleModule>.GetContainer();

        [Test]
        [TestCase(typeof(ISampleService))]
        public void Register(Type type)
        {
            _container.Should().Register(type);
        }

        [Test]
        public void RegisterSampleService()
        {
            _container.Should().Resolve<ISampleService, SampleService>();
        }
    }
}