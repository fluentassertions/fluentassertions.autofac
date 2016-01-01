using System;
using System.Collections.Generic;
using NEdifis.Conventions;
using NUnit.Framework;

namespace Autofac.TestingHelpers
{
    internal class CheckConventions : ConventionBase
    {
        // ReSharper disable once UnusedMember.Local
        private static IEnumerable<Type> TypesToTest { get; } = ClassesToTestFor<CheckConventions>();

        public CheckConventions()
        {
            // NEDifis built-in
            Conventions.AddRange(new IVerifyConvention[]
            {
                new ExcludeFromCodeCoverageClassHasBecauseAttribute(),
                new AllClassesNeedATest(),
                new TestClassesShouldMatchClassToTest(),
                new TestClassesShouldBePrivate(),
            });
        }

        [Test, TestCaseSource(nameof(TypesToTest))]
        public void Check(Type typeToTest)
        {
            Conventions.Check(typeToTest);
        }
    }
}