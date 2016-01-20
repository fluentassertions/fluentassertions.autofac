using System;
using System.Collections.Generic;
using NEdifis.Conventions;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
    internal class CheckConventions : ConventionBase
    {
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