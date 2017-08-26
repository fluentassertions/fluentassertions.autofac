using System;
using Autofac;
using NEdifis.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace FluentAssertions.Autofac
{
	[TestFixtureFor(typeof (ContainerAssertions))]
	// ReSharper disable InconsistentNaming
	internal class ContainerAssertions_Should
	{
		[Test]
		public void Provide_assertions()
		{
			var builder = new ContainerBuilder();
			builder.RegisterInstance(Substitute.For<IDisposable>());
			builder.RegisterType<AutoActivateService>().AutoActivate();
			builder.RegisterType<AutoActivateService2>().AsImplementedInterfaces();
			var container = builder.Build();

			var containerShould = container.Should();

			containerShould.Should().BeOfType<ContainerAssertions>();
			containerShould.Have().Should().BeOfType<ContainerRegistrationAssertions>();
			containerShould.Resolve<IDisposable>().Should().BeOfType<ResolveAssertions>();
			containerShould.Resolve(typeof(IDisposable)).Should().BeOfType<ResolveAssertions>();

			containerShould.AutoActivate<AutoActivateService>();
			//containerShould.AutoActivate<AutoActivateService2>();
			containerShould.Resolve<IStartable>().As<AutoActivateService2>();
			containerShould.Have().Registered<AutoActivateService2>().As<IStartable>();
		}

	    // ReSharper disable ClassNeverInstantiated.Local
		private class AutoActivateService { }
		private class AutoActivateService2 : IStartable {
		    // ReSharper restore ClassNeverInstantiated.Local
			public void Start() { }
		}
	}
}