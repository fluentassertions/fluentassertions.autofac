using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Module = Autofac.Module;

namespace FluentAssertions.Autofac;

/// <inheritdoc />
/// <summary>
///     Contains a number of methods to assert that an <see cref="T:AutoFac.ContainerBuilder" /> is in
///     the expected state.
/// </summary>
#if !DEBUG
    [System.Diagnostics.DebuggerNonUserCode]
#endif
public class BuilderAssertions : ReferenceTypeAssertions<ContainerBuilder, BuilderAssertions>
{
    /// <inheritdoc />
    /// <summary>
    ///     Returns the type of the subject the assertion applies on.
    /// </summary>
    [ExcludeFromCodeCoverage]
    protected override string Identifier => nameof(ContainerBuilder);

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuilderAssertions" /> class.
    /// </summary>
    /// <param name="subject"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BuilderAssertions(ContainerBuilder subject) : base(subject)
    {
    }

    /// <summary>
    ///     Asserts that the specified module type has been registered on the current <see cref="ContainerBuilder" />.
    /// </summary>
    /// <typeparam name="TModule">The module type</typeparam>
    public void RegisterModule<TModule>() where TModule : Module, new()
    {
        RegisterModule(typeof(TModule));
    }

    /// <summary>
    ///     Asserts that the specified module type has been registered on the current <see cref="ContainerBuilder" />.
    /// </summary>
    /// <param name="moduleType">The module type</param>
    public void RegisterModule(Type moduleType)
    {
        EnsureTraversed();
        var module = _modules.FirstOrDefault(m => m.GetType() == moduleType);
        Execute.Assertion
            .ForCondition(module != null)
            .FailWith($"Module '{moduleType}' should be registered but it was not.");
    }

    /// <summary>
    ///     Asserts that the modules contained in the specified assembly have been registered on the current
    ///     <see cref="ContainerBuilder" />.
    /// </summary>
    /// <param name="assembly">The module assembly</param>
    [Obsolete("Use 'RegisterAssemblyModules'")]
    public void RegisterModulesIn(Assembly assembly)
    {
        RegisterModulesOf(assembly);
    }

    /// <summary>
    ///     Asserts that the modules contained in the specified assembly have been registered on the current
    ///     <see cref="ContainerBuilder" />.
    /// </summary>
    /// <param name="assembly">The module assembly</param>
    /// <param name="assemblies">More assemblies to assert</param>
    public void RegisterAssemblyModules(Assembly assembly, params Assembly[] assemblies)
    {
        Enumerable.Repeat(assembly, 1)
            .Concat(assemblies)
            .ToList()
            .ForEach(RegisterModulesOf);
    }

    #region Private

    private void RegisterModulesOf(Assembly assembly)
    {
        var moduleTypes = assembly.GetTypes().Where(t => typeof(Module).IsAssignableFrom(t)).ToList();
        moduleTypes.ForEach(RegisterModule);
    }

    private readonly List<IModule> _modules = new();
    private bool _traversed;

    private void EnsureTraversed()
    {
        if (_traversed)
        {
            return;
        }

        // we will catch all directly registered modules here
        Traverse(Subject);
        // modules loaded indirectly will be registered by the module-builder
        Traverse(_moduleBuilder);
        _traversed = true;
    }

    private void Traverse(ContainerBuilder builder)
    {
        CallbacksOf(builder).ToList().ForEach(Traverse);
    }

    private static IEnumerable<DeferredCallback> CallbacksOf(ContainerBuilder builder)
    {
        return FieldsOf<IEnumerable<DeferredCallback>>(builder)
            .SelectMany(list => list)
            .ToList();
    }

    private void Traverse(DeferredCallback callback)
    {
        Traverse(callback.Callback);
    }

    private void Traverse(Action<IComponentRegistryBuilder> callback)
    {
        switch (callback.Target)
        {
            case Module module:
                _modules.Add(module);
                // load module to our module builder
                Load(module, _moduleBuilder);
                return;
            case IModuleRegistrar registrar:
                CallbacksOf(registrar).ForEach(Traverse);
                return;
        }

        _modules.AddRange(FieldsOf<IModule>(callback.Target));
        CallbacksOf(callback.Target).ForEach(Traverse);
    }


    private readonly ContainerBuilder _moduleBuilder = new();

    private static readonly MethodInfo LoadModule =
        typeof(Module).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Instance);

    private static void Load(Module module, ContainerBuilder builder)
    {
        LoadModule.Invoke(module, new object[] { builder });
    }

    private static List<Action<IComponentRegistryBuilder>> CallbacksOf(object value)
    {
        return FieldsOf<Action<IComponentRegistryBuilder>>(value).ToList();
    }

    private static IEnumerable<T> FieldsOf<T>(object value)
    {
        return value?.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(f => f.GetValue(value))
            .OfType<T>();
    }

    #endregion
}
