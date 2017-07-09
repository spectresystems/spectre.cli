using System.Collections.Generic;
using Shouldly;
using Spectre.CommandLine.Tests.Fixtures;
using Xunit;

namespace Spectre.CommandLine.Tests.Integration
{
    public sealed class CommandAppTests
    {
        public static IEnumerable<object[]> Arguments()
        {
            yield return new object[] { new[] { "foo", "--foo", "Hello", "bar", "--bar", "2" } };
            yield return new object[] { new[] { "foo", "--foo", "Hello", "bar", "-b", "2" } };
            yield return new object[] { new[] { "foo", "-f", "Hello", "bar", "-b", "2" } };
            yield return new object[] { new[] { "foo", "-f", "Hello", "bar", "--bar", "2" } };
        }

        [Theory]
        [MemberData(nameof(Arguments))]
        public void Should_Execute_Command_With_Specified_Options(string[] args)
        {
            // Given
            var fixture = new CommandAppFixture();

            // When
            var result = fixture.Run(args);

            // Then
            result.ShouldBe(0);
            fixture.CallRecorder.WasCalled("FooBar Foo=Hello Bar=2 Qux=0").ShouldBeTrue();
        }

        [Fact]
        public void Should_Use_Default_Value_For_Options_If_Specified()
        {
            // Given
            var fixture = new CommandAppFixture();

            // When
            var result = fixture.Run(new[] { "foo", "--foo", "1", "bar" });

            // Then
            result.ShouldBe(0);
            fixture.CallRecorder.WasCalled("FooBar Foo=1 Bar=3 Qux=0").ShouldBeTrue();
        }

        [Fact]
        public void Should_Use_Type_Converter_For_Options_If_Specified()
        {
            // Given
            var fixture = new CommandAppFixture();

            // When
            var result = fixture.Run(new[] { "foo", "--foo", "1", "bar", "--bar", "1", "--qux", "HelloWorld" });

            // Then
            result.ShouldBe(0);
            fixture.CallRecorder.WasCalled("FooBar Foo=1 Bar=1 Qux=10").ShouldBeTrue();
        }
    }
}
