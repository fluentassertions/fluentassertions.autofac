#if NET45 || NET47 || NETSTANDARD2_0 || NETCOREAPP2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac;
using FluentAssertions.Primitives;

namespace FluentAssertions.Autofac
{
    /// <inheritdoc />
    /// <summary>
    ///     Contains a number of methods to assert registered types on an <see cref="T:Autofac.IContainer" />.
    /// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
    public class TypeScanningAssertions : ReferenceTypeAssertions<IContainer, TypeScanningAssertions>
    {
        /// <summary>
        /// The types.
        /// </summary>
        public readonly IEnumerable<Type> Types;

        private readonly Lazy<List<RegisterAssertions>> _registerAssertions;

        private List<RegisterAssertions> Register => _registerAssertions.Value;

        /// <inheritdoc />
        /// <summary>
        ///     Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => nameof(IContainer);

        /// <summary>
        ///    Initializes a new instance of the <see cref="TypeScanningAssertions" /> class.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="types">The types to assert</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TypeScanningAssertions(IContainer subject, IEnumerable<Type> types)
        {
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Types = FilterTypes(types);
            _registerAssertions = new Lazy<List<RegisterAssertions>>(
                () => Types.Select(t => Subject.Should().Have().Registered(t)).ToList());
        }

        /// <summary>
        /// Specifies a subset of types to register from a scanned assembly using 
        /// the specified <paramref name="predicate"/>.
        /// </summary>
        public TypeScanningAssertions Where(Func<Type, bool> predicate)
        {
            return new TypeScanningAssertions(Subject, Types.Where(predicate));
        }

        /// <summary>
        /// Specifies a subset of types to register from a scanned assembly.
        /// </summary>
        public TypeScanningAssertions Except<T>()
        {
            return Where(t => t != typeof(T));
        }

        /// <summary>
        /// Asserts that the scanned types can be resolved from the current <see cref="IContainer"/> 
        /// as the specified <typeparamref name="T"/>.
        /// </summary>
        public TypeScanningAssertions As<T>()
        {
            return As(typeof(T));
        }

        /// <summary>
        /// Asserts that the scanned types can be resolved from the current <see cref="IContainer"/> 
        /// as the specified <paramref name="type"/>.
        /// </summary>
        public TypeScanningAssertions As(Type type)
        {
            Register.ForEach(r => r.As(type));
            return this;
        }

        /// <summary>
        /// Asserts that the scanned types can be resolved from the current <see cref="IContainer"/> as self.
        /// </summary>
        public TypeScanningAssertions AsSelf()
        {
            Register.ForEach(r => r.AsSelf());
            return this;
        }

        /// <summary>
        /// Asserts that the scanned types can be resolved from the current <see cref="IContainer"/> 
        /// as their implemented interfaces.
        /// </summary>
        public TypeScanningAssertions AsImplementedInterfaces()
        {
            Register.ForEach(r => r.AsImplementedInterfaces());
            return this;
        }

        /// <summary>
        /// Asserts that the scanned types can be resolved from the current <see cref="IContainer"/> 
        /// as the type returned using the specified lambda.
        /// </summary>
        public TypeScanningAssertions As(Func<Type,Type> lambda)
        {
            Register.ForEach(r => r.As(lambda(r.Type)));
            return this;
        }

        private static IEnumerable<Type> FilterTypes(IEnumerable<Type> types)
        {
            return types.Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.GetTypeInfo().IsAbstract &&
                !t.GetTypeInfo().IsGenericTypeDefinition &&
                !IsDelegate(t) &&
                !IsCompilerGenerated(t));
        }

        private static bool IsDelegate(Type type)
        {
            return type.GetTypeInfo().IsSubclassOf(typeof(Delegate));
        }

        private static bool IsCompilerGenerated(Type type)
        {
            return type.GetTypeInfo().GetCustomAttributes<CompilerGeneratedAttribute>().Any();
        }
    }
}
#endif
