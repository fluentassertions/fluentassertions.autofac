using Autofac;

namespace SampleLib
{
    public class SampleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SampleService>().As<ISampleService>();
        }
    }
}
