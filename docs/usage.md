# Usage
 
 Available on [Nuget](https://www.nuget.org) under [FluentAssertions.Autofac](https://www.nuget.org/packages/FluentAssertions.Autofac/)

    Install-Package FluentAssertions.Autofac

Testing registration:

    container.ShouldHave().Registered<SuperCoolService>()
        .AsSelf()
        .As<ICoolService>()
        .Singleton();

    container.ShouldHave().Registered(superCoolInstance);

    container.ShouldHave().Registered<OtherCoolService>()
                .Named<ICoolService<("prettyCool");

    container.ShouldHave().NotRegistered<UncoolService>("uncool");

Testing you can actually resolve an instance which verifies your registration is complete:

    container.ShouldResolve<ICoolService>().As<SuperCoolService>()
        .AutoActivate();

