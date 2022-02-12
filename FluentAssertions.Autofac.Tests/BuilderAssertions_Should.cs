using Autofac;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class BuilderAssertions_Should
    {
        [Fact]
        public void Support_testing_assembly_modules_registrations()
        {
            var builder = new ContainerBuilder();

            var assembly = typeof(BuilderAssertions_Should)
                .Assembly;
            builder.RegisterAssemblyModules(assembly);

            builder.Should().RegisterAssemblyModules(assembly);
            builder.Should().RegisterModule<SampleModule>();
            builder.Should().RegisterModule<SampleModule2>();

#pragma warning disable 618
            builder.Should().RegisterModulesIn(assembly);
#pragma warning restore 618
        }

        [Fact]
        public void Support_testing_module_registration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<SampleModule>();

            var builderShould = builder.Should();
            builderShould.RegisterModule<SampleModule>();
            builderShould.RegisterModule(typeof(SampleModule));

            builderShould.RegisterModule<SampleModule2>();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public class SampleModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterModule<SampleModule2>();
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public class SampleModule2 : Module
        {
        }
    }
}
