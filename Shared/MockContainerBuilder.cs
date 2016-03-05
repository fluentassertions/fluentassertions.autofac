using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;

namespace FluentAssertions.Autofac
{
    public class MockContainerBuilder : ContainerBuilder
    {
        public override void RegisterCallback(Action<IComponentRegistry> configurationCallback)
        {
            base.RegisterCallback(configurationCallback);

            Callbacks.Add(configurationCallback);
        }

        public List<Action<IComponentRegistry>> Callbacks { get; private set; } = new List<Action<IComponentRegistry>>();
    }
}