# Usage
 
 Available on [Nuget](https://www.nuget.org) under [FluentAssertions.Autofac](https://www.nuget.org/packages/FluentAssertions.Autofac/)

    Install-Package FluentAssertions.Autofac

## Test Registration

Testing your DI configuration logic works by testing against your container:

	var container = Configure();
	container.ShouldHave().Registered<SuperCoolService>()
        .AsSelf()
        .As<ICoolService>()
        .Singleton();

    container.ShouldHave().Registered(superCoolInstance);

    container.ShouldHave().Registered<OtherCoolService>()
                .Named<ICoolService<("prettyCool");

    container.ShouldHave().NotRegistered<UncoolService>("uncool");

## Test Resolving
	
You can also test actually resolving an instance which verifies your registration is complete:

    container.ShouldResolve<ICoolService>().As<SuperCoolService>()
        .AutoActivate();

## Testing Autofac Modules

With an application growing in complexity you want to modularize your configuration logic with [Autofac Modules](http://autofac.readthedocs.org/en/latest/configuration/modules.html). Testing your modules is then achieved using the `Module<TModule>` helper class which supports creating isolated & testable builder and containers.

Here is a simple example verifying that the module `MyCoolModule` registers the `SuperCoolService` implementation

    Module<MyCoolModule>.GetTestContainer()
	    .ShouldHave().Registered<SuperCoolService>()
		...

You can also retreive a testable builder like this

    Module<MyCoolModule>.GetTestBuilder()
	    .ShouldHave().RegisteredModule<SomeOtherModule>()
		...

In general, when implementing modules you will typically be subclassing Autofac's module base class `Module` which requires a parameterless constructor. And this is fine in most cases. 

However, we have had some more special use cases where we definitely 
wanted some module to take constructor parameters. In this you cannot use

    builder.RegisterModule<TModule>();

Instead you will use the instance variant

    builder.RegisterModule(module); 

The way to test this is then by using the `Container(...)` and `Builder(...)` extension methods

    var sut = new MyModule(...);
    sut.Container(arrange: ..., types: ...)
       .Should...

    sut.Builder(arrange: ..., types: ...)

### Mocking module dependencies

In case your module depends on some cross cutting dependencies you need some extra arrangement for the testing container to work with your module. 

You can inject an action to arrange substitutes like this

    var container = Module<MyCoolModule>.GetTestContainer(builder =>
    {
        builder.RegisterInstance(Substitute.For<IDependency>());
        ...
    });

Or even simpler just specifying their types

    var container = Module<MyCoolModule>.GetTestContainer(types: new[]
    {
        typeof(IDependency),
        ...
    });


### Testing a module register some other modules 

Say one thing your module does is registering some other modules like this

    builder.RegisterModule<ModuleA>();
    builder.RegisterAssemblyModules(typeof(MyType).Assembly);

This is a use case where you verify that some callbacks have been registered not on the container but on the builder instead

    Module<MyCoolModule>.GetTestBuilder().ShouldHave()
        .RegisteredModule<ModuleA>()
        .RegisteredModulesIn(typeof(MyType).Assembly);
