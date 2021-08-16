using Autofac;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class BuilderWrapperAssertions_Should
    {
        [Fact]
        public void Support_testing_assembly_modules_registrations()
        {
            var wrapper = new BuilderWrapper();

            var assembly = typeof(BuilderWrapperAssertions_Should)
                .Assembly;
            wrapper.Builder.RegisterAssemblyModules(assembly);

            wrapper.Should().RegisterAssemblyModules(assembly);
            wrapper.Should().RegisterModule<SampleModule>();
            wrapper.Should().RegisterModule<SampleModule2>();

#pragma warning disable 618
            wrapper.Should().RegisterModulesIn(assembly);
#pragma warning restore 618
        }

        [Fact]
        public void Support_testing_module_registration()
        {
            var wrapper = new BuilderWrapper();
            wrapper.Builder.RegisterModule<SampleModule>();

            var builderShould = wrapper.Should();
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
