using System;
using System.Collections.Generic;
using Autofac;

namespace AutoFac.TestingHelpers
{
    public static class AutoFacExtensions
    {
        public static void Substitute(this ContainerBuilder builder, IEnumerable<Type> types)
        {
            foreach (var type in types)
                Substitute(builder, type);
        }

        public static void Substitute(this ContainerBuilder builder, Type type)
        {
            builder.RegisterInstance(NSubstitute.Substitute.For(new[] {type}, new object[] {}))
                .AsImplementedInterfaces().AsSelf();
        }

        public static void Substitute<T>(this ContainerBuilder builder) where T : class
        {
            builder.RegisterInstance(NSubstitute.Substitute.For<T>())
                .AsImplementedInterfaces().AsSelf();
        }
    }
}