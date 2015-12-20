using System;
using System.Collections.Generic;
using NEdifis.Conventions;

namespace AutoFac.TestingHelpers
{
    internal class CheckConventions : ConventionBase
    {
        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<Type> TypesToTest { get; } = ClassesToTestFor<CheckConventions>();
    }
}