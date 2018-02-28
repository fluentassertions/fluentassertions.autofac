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

In general, the more you apply [Dependency Injection (DI)](http://martinfowler.com/articles/injection.html) the easier becomes unit testing and [Test-driven Development (TDD)](https://en.wikipedia.org/wiki/Test-driven_development).

This is because the complexity of constructing all dependencies is shifted to the so called [Composition Root](http://blog.ploeh.dk/2011/07/28/CompositionRoot/), i.e. the place where you "wire up" and configure all your dependencies.
Undoubtedly, the best way to do this is by using some [Inversion of Control (IoC)](http://martinfowler.com/articles/injection.html) container.

With an application growing in complexity, there is also growing need to organize and test the IoC configuration.

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
