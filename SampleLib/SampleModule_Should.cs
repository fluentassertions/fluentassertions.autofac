using System;
using AutoFac.TestingHelpers;
using NEdifis.Attributes;
using NUnit.Framework;

namespace SampleLib
{
    [TestFixtureFor(typeof(SampleModule))]
    // ReSharper disable once InconsistentNaming
    internal class SampleModule_Should
    {
        [Test]
        [TestCase(typeof(ISampleService))]
        public void Register(Type type)
        {
            Module<SampleModule>.GetContainer()
                .ShouldRegister(type);
        }

        [Test]
        public void RegisterSampleService()
        {
            Module<SampleModule>.GetContainer()
                .ShouldResolve<ISampleService, SampleService>();
        }
    }
}