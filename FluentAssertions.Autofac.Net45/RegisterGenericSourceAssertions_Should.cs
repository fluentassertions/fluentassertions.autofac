using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using NEdifis.Attributes;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    [TestFixtureFor(typeof(RegisterGenericSourceAssertions))]
    // ReSharper disable InconsistentNaming
    internal class RegisterGenericSourceAssertions_Should
    {
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
