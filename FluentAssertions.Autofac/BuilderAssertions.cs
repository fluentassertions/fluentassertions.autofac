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
        EnsureVisited();
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
    private bool _visited;

    private void EnsureVisited()
    {
        if (_visited)
            return;

        // we will catch all directly registered modules here
        Visit(Subject);

        // modules loaded indirectly will be registered on the module-builder,
        // loop until no new modules discovered
        var modulesVisited = 0;
        while (_modules.Count > modulesVisited)
        {
            Visit(_moduleBuilder);
            modulesVisited = _modules.Count;
        }

        _visited = true;
    }

    private void Visit(ContainerBuilder builder)
    {
        CallbacksOf(builder).ToList().ForEach(Visit);
    }

    private void Visit(DeferredCallback callback)
    {
        Visit(callback.Callback);
    }

    private void Visit(Action<IComponentRegistryBuilder> callback)
    {
        switch (callback.Target)
        {
            case Module module:
                if (!_modules.Contains(module))
                {
                    _modules.Add(module);
                    Load(module, _moduleBuilder);
                }

                return;
            case IModuleRegistrar registrar:
                CallbacksOf(registrar).ForEach(Visit);
                return;
        }

        _modules.AddRange(FieldsOf<IModule>(callback.Target));
        CallbacksOf(callback.Target).ForEach(Visit);
    }

    private static IEnumerable<DeferredCallback> CallbacksOf(ContainerBuilder builder)
    {
        return FieldsOf<IEnumerable<DeferredCallback>>(builder)
            .SelectMany(list => list)
            .ToList();
    }

    private static List<Action<IComponentRegistryBuilder>> CallbacksOf(object value)
    {
        return FieldsOf<Action<IComponentRegistryBuilder>>(value).ToList();
    }

    private readonly ContainerBuilder _moduleBuilder = new();

    private static readonly MethodInfo LoadModule =
        typeof(Module).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Instance);

    private static void Load(Module module, ContainerBuilder builder)
    {
        LoadModule.Invoke(module, new object[] { builder });
    }

    private static IEnumerable<T> FieldsOf<T>(object value)
    {
        return value?.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Select(f => f.GetValue(value))
            .OfType<T>();
    }

    #endregion
}
