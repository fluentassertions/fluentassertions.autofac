using System;
using System.Collections.Generic;
using Autofac;
using NEdifis.Attributes;
using NSubstitute;

namespace AutoFac.TestingHelpers
{
    [ExcludeFromConventions(because: "this is a class for testing")]
    public static class AutoFacExtensions
    {
        public static void RegisterSubstitutes(this ContainerBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
                RegisterSubstitute(builder, type);
        }

        public static void RegisterSubstitute(this ContainerBuilder builder, Type type)
        {
            builder.RegisterInstance(Substitute.For(new[] {type}, new object[] {}))
                .AsImplementedInterfaces().AsSelf();
        }

        public static void RegisterSubstitute<T>(this ContainerBuilder builder) where T : class
        {
            builder.RegisterInstance(Substitute.For<T>())
                .AsImplementedInterfaces().AsSelf();
        }
    }
}