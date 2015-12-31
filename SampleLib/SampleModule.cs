using Autofac;
using NSubstitute;

namespace SampleLib
{
    public class SampleModule : Module
    {
        internal static readonly ISampleInstance SampleInstance = Substitute.For<ISampleInstance>();

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SampleService>().As<ISampleService>();

            builder.RegisterInstance(SampleInstance).SingleInstance();
        }
    }
}
