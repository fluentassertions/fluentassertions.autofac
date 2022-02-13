# Why test your IoC configuration in the first place?

In general, the more you apply [Dependency Injection (DI)](http://martinfowler.com/articles/injection.html) the easier
becomes unit testing and [Test-driven Development (TDD)](https://en.wikipedia.org/wiki/Test-driven_development).

This is because the complexity of constructing all dependencies is shifted to the so
called [Composition Root](http://blog.ploeh.dk/2011/07/28/CompositionRoot/), i.e. the place where you "wire up" and
configure all your dependencies. Undoubtedly, the best way to do this is by using
some [Inversion of Control (IoC)](http://martinfowler.com/articles/injection.html) container.

With an application growing in complexity, there is also growing need to organize and test the IoC configuration.

**Here is the usual story:** During release sprints we typically develop features in parallel that form complete
workflows (think [epics or themes](https://www.mountaingoatsoftware.com/blog/stories-epics-and-themes), if you will).
While carving these out, we mock out dependencies to the other features. This way everyone can move forward without
needing the other features completed.

For example, say you're designing the coolest recommendation view on the planet. While your buddy is on the
recommendation service on the backend, you will register a mock, i.e.

```csharp
builder.RegisterType<MockRecommendations>().As<IGetRecommendations>();
// builder.RegisterType<RecommendationService>().As<IGetRecommendations>();
```

Once your buddy gives thumbs up for integration testing you switch to the real thing. Finally, you finish and move on to
the next feature.

So far, so good. But then, a **critical issue** with your view (or likewise viewmodel, controller) rises in production.
No problem, you go for the hotfix, switch back to your mock so you can better and quicker reproduce the problem. You fix
the problem **but then, you forget to switch back from the mock to the productive recommendation service** - which gives
you a serious [#facepalm](https://en.wikipedia.org/wiki/Facepalm).

![Facepalm (Wikipedia)](https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Paris_Tuileries_Garden_Facepalm_statue.jpg/300px-Paris_Tuileries_Garden_Facepalm_statue.jpg)

Wouldn't it be nice if there was a test saving you from breaking the release next time? Well, now there is.

## Fluent interfaces and readable code

Most of the time, we prefer using this stack

- [Autofac](http://autofac.org/) for wiring up DI,
- [NSubstitute](http://nsubstitute.github.io/) for mocking and
- [FluentAssertions](http://www.fluentassertions.com/) for extremely readable tests that naturally explain when failing.

There is nothing too special about this choice. But there is at least one thing these libraries have in common: They all
share a profound intent on improving accessibility and readability by providing
a [fluent API](https://en.wikipedia.org/wiki/Fluent_interface).

In the rare case that you - as a developer - never quoted that *"Code is read much more often than it is written"* and
are wondering why you should thrive for readable code in the first place, then here is a hand-picked selection of
classic references that we found to be helpful

- [What Do Programmers Really Do Anyway? (Peter Hallam, 2006)](http://blogs.msdn.com/b/peterhal/archive/2006/01/04/509302.aspx)
- [When Understanding means Rewriting (Jeff Atwood, 2006)](http://blog.codinghorror.com/when-understanding-means-rewriting/)
- [Code is read much more often than it is written, so plan accordingly (R.Chen - MSFT, 2007)](https://blogs.msdn.microsoft.com/oldnewthing/20070406-00/?p=27343)
- [Clean Code: A Handbook of Agile Software Craftsmanship (Robert C. Martin et al., 2008)](http://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882)
- [The Futility of Commenting Code (Bob Carpenter, 2009)](http://lingpipe-blog.com/2009/10/15/the-futility-of-commenting-code/)
- [Learn to Read the Source, Luke (J.Atwood 2012)](http://blog.codinghorror.com/learn-to-read-the-source-luke/)
- [Review of M.Fowlers classic "Refactoring: Improving the Design of Existing Code" (2012)](http://siderite.blogspot.com/2012/04/refactoring-improving-design-of.html)
