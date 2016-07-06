using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;

namespace FluentAssertions.Autofac
{
    /// <summary>
    ///   A testable <see cref="ContainerBuilder"/> that exposes the callbacks registered on the builder.
    /// </summary>
    public class MockContainerBuilder : ContainerBuilder
    {
        /// <summary>
        /// Register a callback that will be invoked when the container is configured.
        /// </summary>
        /// <remarks>
        /// This is primarily for extending the builder syntax.
        /// </remarks>
        /// <param name="configurationCallback">Callback to execute.</param>
        public override void RegisterCallback(Action<IComponentRegistry> configurationCallback)
        {
            base.RegisterCallback(configurationCallback);

            Callbacks.Add(configurationCallback);
        }

        /// <summary>
        ///   The callbacks that have been registered on the builder.
        /// </summary>
        public List<Action<IComponentRegistry>> Callbacks { get; } = new List<Action<IComponentRegistry>>();
    }
}