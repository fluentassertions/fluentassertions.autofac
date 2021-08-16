using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Xunit;

namespace FluentAssertions.Autofac
{
    // ReSharper disable InconsistentNaming
    public class RegisterGenericSourceAssertions_Should
    {
        [Fact]
        public void Register_Generic()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            containerShouldHave
                .RegisteredGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .SingleInstance();
        }

        [Fact]
        public void Register_Generic_WithMultiple_GenericArgumentTypes()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(MultipleRepository<,>))
                    .As(typeof(IMultipleRepository<,>))
                    .SingleInstance()
            );

            containerShouldHave
                .RegisteredGeneric(typeof(MultipleRepository<,>))
                .As(typeof(IMultipleRepository<,>))
                .SingleInstance();
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentNullException_WhenGenericComponentTypeDefinition_IsNull()
        {
            AssertRegisterGenericThrows<ArgumentNullException>(null, typeof(IRepository<>));
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentException_WhenGenericComponentTypeDefinition_IsNotGenericType()
        {
            AssertRegisterGenericThrows(typeof(NotGenericRepository), typeof(IRepository<>));
        }

        [Fact]
        public void
            Register_Generic_ShouldThrow_ArgumentException_WhenGenericComponentTypeDefinition_IsNotGenericTypeDefinition()
        {
            AssertRegisterGenericThrows(typeof(IRepository<object>), typeof(IRepository<>));
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentNullException_WhenGenericServiceTypeDefinition_IsNull()
        {
            AssertRegisterGenericThrows<ArgumentNullException>(typeof(IRepository<>), null);
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentException_WhenGenericServiceTypeDefinition_IsNotGenericType()
        {
            AssertRegisterGenericThrows(typeof(IRepository<>), typeof(IRepository));
        }

        [Fact]
        public void
            Register_Generic_ShouldThrow_ArgumentException_WhenGenericServiceTypeDefinition_IsNotGenericTypeDefinition()
        {
            AssertRegisterGenericThrows(typeof(IRepository<>), typeof(IRepository<object>));
        }

        private static void AssertRegisterGenericThrows(Type componentType, Type serviceType)
        {
            AssertRegisterGenericThrows<ArgumentException>(componentType, serviceType);
        }

        private static void AssertRegisterGenericThrows<TException>(Type componentType, Type serviceType)
            where TException : ArgumentException
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            containerShouldHave.Invoking(x => x
                .RegisteredGeneric(componentType)
                .As(serviceType)
                .SingleInstance()
            ).Should().Throw<TException>();
        }

        private static ContainerRegistrationAssertions GetSut(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should().Have();
        }

        private interface IRepository
        {
        }

        [ExcludeFromCodeCoverage]
        private class Repository<TEntity> : IRepository<TEntity>
        {
        }

        [ExcludeFromCodeCoverage]
        private class MultipleRepository<TEntity1, TEntity2> : IMultipleRepository<TEntity1, TEntity2>
        {
        }

        [ExcludeFromCodeCoverage]
        private class NotGenericRepository : IRepository<object>
        {
        }

        // ReSharper disable UnusedTypeParameter
        private interface IRepository<TEntity> : IRepository
        {
        }

        private interface IMultipleRepository<TEntity1, TEntity2> : IRepository
        {
        }
        // ReSharper restore UnusedTypeParameter
    }
}
