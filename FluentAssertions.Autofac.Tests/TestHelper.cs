namespace FluentAssertions.Autofac;

internal static class TestHelper
{
    public static void ShouldBeOfType<T>(this object value)
    {
        value.Should().BeOfType<T>();
    }
}
