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
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            Action action = () =>
            {
                containerShouldHave
                    .RegisteredGeneric(null)
                    .As(typeof(IRepository<>))
                    .SingleInstance();
            };


            action.Should().Throw<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("genericComponentTypeDefinition");
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentException_WhenGenericComponentTypeDefinition_IsNotGenericType()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            var genericComponentTypeDefinition = typeof(NotGenericRepository);
            Action action = () =>
            {
                containerShouldHave
                    .RegisteredGeneric(genericComponentTypeDefinition)
                    .As(typeof(IRepository<>))
                    .SingleInstance();
            };


            action.Should().Throw<ArgumentException>()
                .And
                .ParamName
                .Should()
                .Be(nameof(genericComponentTypeDefinition));
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentException_WhenGenericComponentTypeDefinition_IsNotGenericTypeDefinition()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            var genericComponentTypeDefinition = typeof(Repository<object>);
            Action action = () =>
            {
                containerShouldHave
                    .RegisteredGeneric(genericComponentTypeDefinition)
                    .As(typeof(IRepository<>))
                    .SingleInstance();
            };

            action.Should().Throw<ArgumentException>()
                .And
                .ParamName
                .Should()
                .Be(nameof(genericComponentTypeDefinition));
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentNullException_WhenGenericServiceTypeDefinition_IsNull()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            Action action = () =>
            {
                containerShouldHave
                    .RegisteredGeneric(typeof(Repository<>))
                    .As(null)
                    .SingleInstance();
            };

            action.Should().Throw<ArgumentNullException>()
                .And
                .ParamName
                .Should()
                .Be("genericServiceTypeDefinition");
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentException_WhenGenericServiceTypeDefinition_IsNotGenericType()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            var genericServiceTypeDefinition = typeof(IRepository);
            Action action = () =>
            {
                containerShouldHave
                    .RegisteredGeneric(typeof(Repository<>))
                    .As(genericServiceTypeDefinition)
                    .SingleInstance();
            };

            action.Should().Throw<ArgumentException>()
                .And
                .ParamName
                .Should()
                .Be(nameof(genericServiceTypeDefinition));
        }

        [Fact]
        public void Register_Generic_ShouldThrow_ArgumentException_WhenGenericServiceTypeDefinition_IsNotGenericTypeDefinition()
        {
            var containerShouldHave = GetSut(builder =>
                builder.RegisterGeneric(typeof(Repository<>))
                    .As(typeof(IRepository<>))
                    .SingleInstance()
            );

            var genericServiceTypeDefinition = typeof(IRepository<object>);
            Action action = () =>
            {
                containerShouldHave
                    .RegisteredGeneric(typeof(Repository<>))
                    .As(genericServiceTypeDefinition)
                    .SingleInstance();
            };

            action.Should().Throw<ArgumentException>()
                .And
                .ParamName
                .Should()
                .Be(nameof(genericServiceTypeDefinition));
        }

        private static ContainerRegistrationAssertions GetSut(Action<ContainerBuilder> arrange = null)
        {
            var builder = new ContainerBuilder();
            arrange?.Invoke(builder);
            return builder.Build().Should().Have();
        }

        private interface IRepository { }

        private interface IRepository<TEntity> : IRepository { }

        private interface IMultipleRepository<TEntity1, TEntity2> : IRepository { }

        [ExcludeFromCodeCoverage]
        private class Repository<TEntity> : IRepository<TEntity> { }

        [ExcludeFromCodeCoverage]
        private class MultipleRepository<TEntity1, TEntity2> : IMultipleRepository<TEntity1, TEntity2> { }

        [ExcludeFromCodeCoverage]
        private class NotGenericRepository : IRepository<object> { }
    }
}
