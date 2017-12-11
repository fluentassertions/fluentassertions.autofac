
# FluentAssertions.AutoFac

[![Build status](https://ci.appveyor.com/api/projects/status/u42b929walkd6086?svg=true)](https://ci.appveyor.com/project/awesome-inc-build/fluentassertions-autofac)
[![NuGet](https://img.shields.io/nuget/v/FluentAssertions.Autofac.svg?style=flat-square)](https://www.nuget.org/packages/FluentAssertions.Autofac/)
[![NuGet](https://img.shields.io/nuget/dt/FluentAssertions.Autofac.svg?style=flat-square)](https://www.nuget.org/packages/FluentAssertions.Autofac/)
[![Coverage Status](https://coveralls.io/repos/github/awesome-inc/FluentAssertions.Autofac/badge.svg)](https://coveralls.io/github/fluentassertions/fluentassertions.autofac)
[![Documentation Status](https://readthedocs.org/projects/fluentassertionsautofac/badge/?version=latest)](http://fluentassertionsautofac.rtfd.io/en/latest/)

This repository contains the [Fluent Assertions](http://fluentassertions.com/) extensions for [AutoFac](https://autofac.org/).
It is maintained by [@mkoertgen](https://github.com/mkoertgen).

* See [www.fluentassertions.com](http://www.fluentassertions.com/) for more information about the main library.

## Why?

To test Ninject mappings using the [conventions extension](https://github.com/ninject/ninject.extensions.conventions) (or setup manually) are resolving.  Instead of finding out at runtime that your app won't run, you'll have a failing unit test instead.

Tests are written like this:

```` c#
[Test]
public void Services_can_be_resolved_with_a_single_instance()
{
    // Arrange
    var kernel = GetKernel();
    var interfaces = FindAssembly.Containing<ISampleService>().GetPublicInterfaces()
                                                              .EndingWith("Service");

    // Assert
    kernel.Should().Resolve(interfaces).WithSingleInstance();
}
````

## Quickstart

### Usage

Install the NuGet package

```powershell
PM> Install-Package FluentAssertions.AutoFac
```

and start writing tests for your AutoFac configuration.

```csharp
container.Should().Have().Registered<SuperCoolService>()
    .AsSelf()
    .As<ICoolService>()
    .Singleton();
```

Find more examples in the documentation our the tests.

### How to build

Clone and build using Visual Studio or the command line using [OneClickBuild](https://github.com/awesome-inc/OneClickBuild)

```console
git clone https://github.com/fluentassertions/fluentassertions.autofac.git
cd fluentassertions.autofac
build
```

### Links

* [Why?](http://fluentassertionsautofac.readthedocs.org/en/latest/why)
* [Usage](http://fluentassertionsautofac.readthedocs.org/en/latest/usage/)
* [Documentation](http://fluentassertionsautofac.readthedocs.io/en/latest)
* [Contributing](https://github.com/fluentassertions/fluentAssertions.autofac/blob/develop/CONTRIBUTING.md)
