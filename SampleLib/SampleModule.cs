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

            builder.RegisterType<NamedInstance>().Named<INamedInstance>("SampleName");
            builder.RegisterType<OnlineState>().Keyed<IDeviceState>(DeviceState.Online);
            builder.RegisterType<SampleStarter>().AutoActivate();
        }
    }
}
