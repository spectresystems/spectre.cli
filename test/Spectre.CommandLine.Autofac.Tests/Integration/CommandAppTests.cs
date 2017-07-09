using Shouldly;
using Spectre.CommandLine.Autofac.Tests.Fixtures;
using Xunit;

namespace Spectre.CommandLine.Autofac.Tests.Integration
{
    public sealed class CommandAppTests
    {
        [Fact]
        public void Should_Execute_Command_With_Specified_Options()
        {
            // Given
            var fixture = new CommandAppFixture();

            // When
            var result = fixture.Run(new[] { "foo", "-f", "Hello", "bar", "--bar", "2" });

            // Then
            result.ShouldBe(0);
            fixture.CallRecorder.WasCalled("FooBar Foo=Hello Bar=2 Qux=0").ShouldBeTrue();
        }
    }
}
