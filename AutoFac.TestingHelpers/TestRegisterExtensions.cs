using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentAssertions;
using NEdifis.Attributes;
using Module = Autofac.Module;

namespace AutoFac.TestingHelpers
{
    [ExcludeFromCodeCoverage]
    [ExcludeFromConventions("testing helper")]
    public static class TestRegisterExtensions
    {
        public static IContainer GetContainer<TModule>(this TModule module, 
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
            where TModule : Module, new()
        {
            var builder = GetBuilder(module, arrange, types);
            return builder.Build();
        }

        public static MockContainerBuilder GetBuilder<TModule>(this TModule module, 
            Action<ContainerBuilder> arrange = null, IEnumerable<Type> types = null)
            where TModule : Module, new()
        {
            var builder = new MockContainerBuilder();
            if (types != null) builder.RegisterSubstitutes(types);
            arrange?.Invoke(builder);
            builder.RegisterModule(module);
            return builder;
        }

        public static void ShouldResolve<TRegister>(this IContainer container, params Type[] resolvedTypes)
        {
            container.IsRegistered<TRegister>().Should().BeTrue();

            if (resolvedTypes == null || !resolvedTypes.Any()) return;

            var instances = container.Resolve<IEnumerable<TRegister>>().ToArray();
            foreach (var type in resolvedTypes)
            {
                var instance = instances.FirstOrDefault(i => i.GetType() == type);
                instance.Should().NotBeNull($"Type '{type}' should be registered as '{typeof(TRegister)}'");
            }
        }

        public static void ShouldResolve<TRegister, TResolve>(this IContainer container) where TResolve : TRegister
        {
            container.ShouldResolve<TRegister>(typeof(TResolve));
        }

        public static void ShouldResolveTo<TRegister>(this IContainer container, object expected)
        {
            container.Resolve<TRegister>().Should().Be(expected);
        }

        public static void ShouldRegister<TRegister>(this IContainer container, string because = null)
        {
            container.ShouldRegister(typeof(TRegister), because);
        }

        public static void ShouldRegister(this IContainer container, Type type, string because = null)
        {
            container.IsRegistered(type).Should().BeTrue(because ?? $"Type '{type}' should be registered but it is not.");
        }

        public static void ShouldRegister(this IContainer container, IEnumerable<Type> types)
        {
            types.All(container.IsRegistered).Should().BeTrue();
        }

        public static void ShouldAutoActivate<TRegister>(this IContainer container) where TRegister : IStartable
        {
            ShouldResolve<IStartable, TRegister>(container);
        }

        public static void ShouldAutoActivate(this IContainer container, params Type[] types)
        {
            ShouldResolve<IStartable>(container, types);
        }

        public static void ShouldRegisterAllModulesInAssembly(this MockContainerBuilder builder, Assembly assembly)
        {
            var expectedModules = assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t)).ToArray();

            var callbacks = builder.Callbacks;
            callbacks.Count.Should().BeGreaterOrEqualTo(expectedModules.Length);

            foreach (var callback in callbacks)
            {
                var module = callback.Target as Module;
                if (module != null) callback.Method.Name.Should().Be(nameof(Module.Configure));
            }
        }
    }
}